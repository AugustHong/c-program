#include<stdio.h>
#include<stdlib.h>

#define max 10

int data[max][10] = { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2,   //��ɬO2 �´ѬO-1 �մѬO1 �Ů�O0
					 2, 1, 1, 1, 0, 0, 0, 0, 0, 2,
					 2, 1, -1, 1, 0, 0, 0, 0, 0, 2,
					 2, 1, -1, 1, 0, 0, 0, 0, 0, 2,
					 2, 1, 0, 1, 0, 0, 1, 0, 0, 2,
					 2, 0, 0, 0, 0, 1, -1, 1, 0, 2,
					 2, 0, 0, 0, 1, -1, -1, -1, 1, 2,
					 2, 0, 0, 0, 0, 1, -1, 0, 0, 2,
					 2, 0, 0, 0, 0, 0, 0, 0, 0, 2,
					 2, 2, 2, 2, 2, 2, 2, 2, 2, 2 };

int record[30][2];
int current = 0, a, b;

void print();
bool exit(int x, int y);
bool judge(int x, int y, int color);
void clear(bool a);

int main() {
	puts("���խ�(4,2) (8,6)");
	print();

	printf("�п�J2�ӼƦr\n");
	scanf("%d %d", &a, &b);
	data[a][b] = 1;

	clear(judge(a + 1, b, -1));
	clear(judge(a - 1, b, -1));
	clear(judge(a, b + 1, -1));
	clear(judge(a, b - 1, -1));

	print();

	system("pause");
	return 0;
}

void print() {
	puts("   0  1  2  3  4  5  6  7  8  9");
	for (int i = 0; i < max; i++) {
		printf("%d  ", i);
		for (int j = 0; j < max; j++) {
			if (data[i][j] == 0) { printf("�� "); }
			if (data[i][j] == 2) { printf("�� "); }
			if (data[i][j] == 1) { printf("�� "); }
			if (data[i][j] == -1) { printf("�� "); }
		}
		printf("\n");
	}
}

bool exit(int x, int y) {
	for (int i = 0; i < 30; i++) {
		if (record[i][0] == x && record[i][1] == y) { return false; }
	}

	return true;
}

void clear(bool a) { //��������M��

	if (a){
		for (int i = 0; i <= current; i++){ record[i][0] = 0; record[i][1] = 0;} //�N����X�f�٬O�n��record���M��
	}
	else {
		for (int i = 0; i <= current; i++) {
			if (record[i][0] != 0 && record[i][1] != 0) {
				data[record[i][0]][record[i][1]] = 0;
				printf("�w�M��(%d, %d)\n", record[i][0], record[i][1]);
				record[i][0] = 0;
				record[i][1] = 0;
			}
		}
	}
	current = 0; //clear current
}

bool judge(int x, int y, int color) {

	if (data[x + 1][y] == 0 || data[x - 1][y] == 0 || data[x][y + 1] == 0 || data[x][y - 1] == 0) { return true; }

	printf("��(%d, %d)\n", x, y);
	record[current][0] = x;
	record[current][1] = y;
	current += 1;

	if (data[x + 1][y] == color && exit(x + 1, y)) { if (judge(x + 1, y, color)) { return true; }; }  //�n�P��B����O��L��(�p�G�w���i�H�X�h�F�I���N���ΦA��F)�A�N�����o�ˤ@����즳�X�^�ΨS�X�^����
	if (data[x - 1][y] == color && exit(x - 1, y)) { if ( judge(x - 1, y, color)) { return true; }; }
	if (data[x][y + 1] == color && exit(x, y + 1)) {if (judge(x, y + 1, color)) { return true; };}
	if (data[x][y - 1] == color && exit(x, y - 1)) { if (judge(x, y - 1, color)) { return true; }; }

	return false;
}

