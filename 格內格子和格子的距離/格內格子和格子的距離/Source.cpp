#include<stdio.h>
#include<stdlib.h>

#define max 100

int data[max] = { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,   //邊界是-1, -5是起始點
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
int count_dis(int p, int current_n);  //p是當前位子，而current_n是當前算到的距離數

int main() {

	printf("邊界是-1, -5是起始點\n");
	printf("請輸入距離數字\n");
	scanf("%d", &n);

	if (n > 0) { count_dis(45, 1); }  //從1開始算到n

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
