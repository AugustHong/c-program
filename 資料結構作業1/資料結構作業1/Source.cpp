#include<stdio.h>
#include<stdlib.h>
#include<time.h>
#include<string.h>
#include<conio.h>

#define max 10

struct data {
	int hour;
	int min;
	char text[64];
	bool notified = false;
};

struct data Alarm[max], store;
struct tm *tm1; // 時間的一個結構
time_t time1; //宣告時間變數
int current_number = 0, total_input_number = 0; //第一個是目前到的數字（這樣可以減少跑的次數），而下一個是總共幾筆資料

void format() {
	char clear[64] = { "" }; //清除用的

	for (int i = 0; i < max; i++) {
		Alarm[i].hour = 24;
		Alarm[i].min = 00;
		strcpy(Alarm[i].text, clear);
		Alarm[i].notified = false;
	}
}

char *judge(bool judge_notified) {
	if (judge_notified) { return "已通知"; }
	else { return "未通知"; }
}

void print() {
	for (int i = 0; i < max; i++) { printf("%02d:%02d  %-64s %s\n", Alarm[i].hour, Alarm[i].min, Alarm[i].text, judge(Alarm[i].notified)); } 
	//%02d的2是一定顯示2個位置，而0是代表空格補0(%-64s的-是代表向左對齊)
	puts("==============================================================================");
}

int clear(int j) { //因為會拿到\n的資料，所以要把它取代掉
	for (int i = 0; i <= 63; i++) { if ((int)Alarm[j].text[i] == 10) { Alarm[j].text[i] = '\0'; } }
	return 0; 
}

void setting() {
	FILE *source = fopen("morningCall.txt", "r");

	while (!feof(source)) {
		fscanf(source, "%d:%d", &Alarm[total_input_number].hour, &Alarm[total_input_number].min);
		//因為後面的字串可能會斷行（所以不行用%d%ds），所以分開做（先取完這3個，而指標現在指在字串第一個位置）
		fgets(Alarm[total_input_number].text, 64, source);//再拿取後面的全部字串	（且指定只能64個字元）	
		clear(total_input_number); //會有\n，要去掉
		total_input_number += 1;
		if (total_input_number == max) { break; } //如果超過10筆就結束了，不管檔案裡有多少
	}

	fclose(source);

	print();
}

int change(int i) { //讓資料交換（這樣只要跑剩下的幾筆，可以省時間和空間）
	store = Alarm[i];
	Alarm[i] = Alarm[current_number];
	Alarm[current_number] = store;
	current_number += 1;
	return 0;
}

void notified_clear() {
	for (int i = 0; i < max; i++) { Alarm[i].notified = false; }
	current_number = 0;
}

void running() {

	while (true) { 
		//取得當前時間
		time(&time1);
		tm1 = localtime(&time1);

		if (_kbhit() && _getch() == 'q') { break; }
		if (tm1->tm_hour == 0 && tm1->tm_min == 0) { notified_clear();  } //如果0:0:0就清空全部

		for (int i = current_number; i < total_input_number; i++) {
			if (tm1->tm_hour == Alarm[i].hour && tm1->tm_min == Alarm[i].min) {
				printf("%02d:%02d  %s 已通知\n", Alarm[i].hour, Alarm[i].min, Alarm[i].text);
				puts("=========================================");
				Alarm[i].notified = true;
				change(i);
				break;
			}
		}
	}
}

int main() {
	format();
	setting();
	running();

	system("pause");
	return 0;
}