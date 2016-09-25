#include<stdio.h>
#include<stdlib.h>
#include<time.h>
#include<string.h>
#include<conio.h>

#define max 10

struct data {
	int hour;
	int min;
	int sec;
	char text[64];
	bool status = false;
};

struct data datalist[max], store;
struct tm *tm1; // 時間的一個結構
time_t time1; //宣告時間變數
int current_number = 0, total_input_number = 0; //第一個是目前到的數字（這樣可以減少跑的次數），而下一個是總共幾筆資料

void format() {
	char clear[64] = { "" }; //清除用的

	for (int i = 0; i < max; i++) {
		datalist[i].hour = 24;
		datalist[i].min = 0;
		datalist[i].sec = 0;
		strcpy(datalist[i].text, clear);
		datalist[i].status = false;
	}
}

char *judge(bool judge_status) {
	if (judge_status) { return "已通知"; }
	else { return "未通知"; }
}

void print() {
	for (int i = 0; i < max; i++) { printf("%d:%d:%d     %s     %s\n", datalist[i].hour, datalist[i].min, datalist[i].sec, datalist[i].text, judge(datalist[i].status)); }
	puts("=========================================");
}

int clear(int j) { //因為會拿到\n的資料，所以要把它取代掉
	for (int i = 0; i <= 63; i++) { if ((int)datalist[j].text[i] == 10) { datalist[j].text[i] = '\0'; } }
	return 0; 
}

void setting() {
	FILE *source = fopen("source.txt", "r");

	while (!feof(source)) {
		fscanf(source, "%d%d%d", &datalist[total_input_number].hour, &datalist[total_input_number].min, &datalist[total_input_number].sec);
		//因為後面的字串可能會斷行（所以不行用%d%d%d%s），所以分開做（先取完這3個，而指標現在指在字串第一個位置）
		fgets(datalist[total_input_number].text, 64, source);//再拿取後面的全部字串	（且指定只能64個字元）	
		clear(total_input_number); //會有\n，要去掉
		total_input_number += 1;
		if (total_input_number == 10) { break; } //如果超過10筆就結束了，不管檔案裡有多少
	}

	fclose(source);

	print();
}

int change(int i) { //讓資料交換（這樣只要跑剩下的幾筆，可以省時間和空間）
	store = datalist[i];
	datalist[i] = datalist[current_number];
	datalist[current_number] = store;
	current_number += 1;
	return 0;
}

void running() {

	while (current_number < total_input_number) { //只要資料還沒跑完就繼續
		//取得當前時間
		time(&time1);
		tm1 = localtime(&time1);

		if (_kbhit() && _getch() == 'q') { break; }
		if (tm1->tm_hour == 0 && tm1->tm_min == 0 && tm1->tm_sec == 0) { format(); break; } //如果0:0:0就清空全部

		for (int i = current_number; i < total_input_number; i++) {
			if (tm1->tm_hour == datalist[i].hour && tm1->tm_min == datalist[i].min && tm1->tm_sec == datalist[i].sec) {
				printf("%d:%d:%d  %s  已通知\n", datalist[i].hour, datalist[i].min, datalist[i].sec, datalist[i].text);
				puts("=========================================");
				datalist[i].status = true;
				change(i);
			}
		}
	}

	format();
}

int main() {
	format();
	setting();
	running();

	system("pause");
	return 0;
}