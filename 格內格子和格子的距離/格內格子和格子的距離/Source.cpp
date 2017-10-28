#include<stdio.h>
#include<stdlib.h>

#define max 100

int data[max] = { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,   //��ɬO-1, -5�O�_�l�I
-1, 0, 0, 0, 0, 0, 0, 0, 0, -1,
-1, 0, 0, 0, 0, 0, 0, 0, 0, -1,
-1, 0, 0, 0, 0, 0, 0, 0, 0, -1,
-1, 0, 0, 0, 0, -5, 0, 0, 0, -1,
-1, 0, 0, 0, 0, 0, 0, 0, 0, -1,
-1, 0, 0, 0, 0, 0, 0, 0, 0, -1,
-1, 0, 0, 0, 0, 0, 0, 0, 0, -1,
-1, 0, 0, 0, 0, 0, 0, 0, 0, -1,
-1, -1, -1, -1, -1, -1, -1, -1, -1, -1 };

int n = 0;

void print();
int count_dis(int p, int current_n);  //p�O��e��l�A��current_n�O��e��쪺�Z����

int main() {

	printf("��ɬO-1, -5�O�_�l�I\n");
	printf("�п�J�Z���Ʀr\n");
	scanf("%d", &n);

	if (n > 0) { count_dis(45, 1); }  //�q1�}�l���n

	print();

	system("pause");
	return 0;
}

void print() {
	for (int i = 0; i <= 99; i++) {
		if (i % 10 == 0) { printf("\n"); }
		printf("%2d", data[i]);		
	}
}

int count_dis(int p, int current_n) {

	if (current_n <= n) {
		if (data[p - 10] == 0 || data[p - 10] > current_n) {  //up
			data[p - 10] = current_n;
			count_dis(p - 10, current_n + 1);
		}

		if (data[p + 10] == 0 || data[p + 10] > current_n) {  //down
			data[p + 10] = current_n;
			count_dis(p + 10, current_n + 1);
		}

		if (data[p - 1] == 0 || data[p - 1] > current_n) {  //left
			data[p - 1] = current_n;
			count_dis(p - 1, current_n + 1);
		}

		if (data[p + 1] == 0 || data[p + 1] > current_n) {  //right
			data[p + 1] = current_n;
			count_dis(p + 1, current_n + 1);
		}
	}
	return 0;
}
