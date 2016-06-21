#include<stdio.h>
#include<stdlib.h>

void clear();
int judge(int x, int y);

int number[17][17], judge_num[17] = { 0 }; //number�O�Ӧs��ƪ��A��judge_number�O�ΨӧP�_�O�_�����ƪ�
int c = -1; //c�O�Ψӷ�@�P�_��X�Ϊ�(0�O�����A1�O�諸)

int main(int argc, char *argv[]) {	
	int i = 1, j = 1; //j�O��i�O�C

	if (argc != 2) { puts("error"); exit(1); }
	FILE *source = fopen(argv[1], "r");

	while (!feof(source)) {
		fscanf(source, "%d", &number[j][i]);
		i++;
		if (i > 16) { i = 1; j++; } //�p�G�W�L16�h���G���}�C���Y+1�A�ۤw�h�ܦ�1
	}

	for (int x = 1; x <= 16; x++) { //�@�Ƥ@�Ƨ�
		for (int y = 1; y <= 16; y++) {
			judge_num[number[x][y]] ++; //���Ʀr�ۤv�[1�A�p�G�����ƫh�|�O2
			if (judge_num[number[x][y]] == 2) {c = 0; break; } //���ƤF�I
		}
		clear(); //�]���@���N�n�M���A���M�|��
		if (c == 0) { break; }
	}


	if (c != 0) {
		for (int x = 1; x <= 16; x++) { //�@�C�@�C��
			for (int y = 1; y <= 16; y++) {
				judge_num[number[y][x]] ++; //���Ʀr�ۤv�[1�A�p�G�����ƫh�|�O2
				if (judge_num[number[y][x]] == 2) { c = 0; break; } //���ƤF�I
			}
			clear(); //�]���@���N�n�M���A���M�|��
			if (c == 0) { break; }
		}
	}


	if (c != 0) {
		for (int x = 1; x <= 13; x += 4) { //�@���@���h��
			for (int y = 1; y <= 13; y += 4) {if (judge(x, y) == 0) { c = 0;  break; }}
			if (c == 0) { break; }
		}
	}

	if (c == 0) { puts("False"); }else { puts("True"); } //��X

	system("pause");
	return 0;
}

void clear() { //�M�^�ȡA���M�|��
	for (int i = 1; i <= 16; i++) { judge_num[i] = 0; }
}

int judge(int x, int y) {
	clear(); //�]���@���N�n�M���A���M�|��

	for (int i = x; i < x+4; i++) {
		for (int j = y; j < y+4; j++) {
			judge_num[number[i][j]] ++; //���Ʀr�ۤv�[1�A�p�G�����ƫh�|�O2
			if (judge_num[number[i][j]] == 2) { return 0; } //���ƤF�I
		}
	}
	return 1;
}