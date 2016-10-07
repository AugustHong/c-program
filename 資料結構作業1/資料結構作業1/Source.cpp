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

struct data Alarm[max], store;
struct tm *tm1; // �ɶ����@�ӵ��c
time_t time1; //�ŧi�ɶ��ܼ�
int current_number = 0, total_input_number = 0; //�Ĥ@�ӬO�ثe�쪺�Ʀr�]�o�˥i�H��ֶ]�����ơ^�A�ӤU�@�ӬO�`�@�X�����

void format() {
	char clear[64] = { "" }; //�M���Ϊ�

	for (int i = 0; i < max; i++) {
		Alarm[i].hour = 24;
		Alarm[i].min = 0;
		Alarm[i].sec = 0;
		strcpy(Alarm[i].text, clear);
		Alarm[i].status = false;
	}
}

char *judge(bool judge_status) {
	if (judge_status) { return "�w�q��"; }
	else { return "���q��"; }
}

void print() {
	for (int i = 0; i < max; i++) { printf("%d:%d:%d     %s     %s\n", Alarm[i].hour, Alarm[i].min, Alarm[i].sec, Alarm[i].text, judge(Alarm[i].status)); }
	puts("=========================================");
}

int clear(int j) { //�]���|����\n����ơA�ҥH�n�⥦���N��
	for (int i = 0; i <= 63; i++) { if ((int)Alarm[j].text[i] == 10) { Alarm[j].text[i] = '\0'; } }
	return 0; 
}

void setting() {
	FILE *source = fopen("morningCall.txt", "r");

	while (!feof(source)) {
		fscanf(source, "%d%d%d", &Alarm[total_input_number].hour, &Alarm[total_input_number].min, &Alarm[total_input_number].sec);
		//�]���᭱���r��i��|�_��]�ҥH�����%d%d%d%s�^�A�ҥH���}���]�������o3�ӡA�ӫ��в{�b���b�r��Ĥ@�Ӧ�m�^
		fgets(Alarm[total_input_number].text, 64, source);//�A�����᭱�������r��	�]�B���w�u��64�Ӧr���^	
		clear(total_input_number); //�|��\n�A�n�h��
		total_input_number += 1;
		if (total_input_number == max) { break; } //�p�G�W�L10���N�����F�A�����ɮ׸̦��h��
	}

	fclose(source);

	print();
}

int change(int i) { //����ƥ洫�]�o�˥u�n�]�ѤU���X���A�i�H�ٮɶ��M�Ŷ��^
	store = Alarm[i];
	Alarm[i] = Alarm[current_number];
	Alarm[current_number] = store;
	current_number += 1;
	return 0;
}

void status_clear() {
	for (int i = 0; i < max; i++) { Alarm[i].status = false; }
	current_number = 0;
}

void running() {

	while (true) { 
		//���o��e�ɶ�
		time(&time1);
		tm1 = localtime(&time1);

		if (_kbhit() && _getch() == 'q') { break; }
		if (tm1->tm_hour == 0 && tm1->tm_min == 0 && tm1->tm_sec == 0) { status_clear();  } //�p�G0:0:0�N�M�ť���

		for (int i = current_number; i < total_input_number; i++) {
			if (tm1->tm_hour == Alarm[i].hour && tm1->tm_min == Alarm[i].min && tm1->tm_sec == Alarm[i].sec) {
				printf("%d:%d:%d  %s  �w�q��\n", Alarm[i].hour, Alarm[i].min, Alarm[i].sec, Alarm[i].text);
				puts("=========================================");
				Alarm[i].status = true;
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