#include<stdio.h>
#include<stdlib.h>
#include<time.h>
#include<math.h>

//插入排序法(別人輸入數字即立刻排序)

int h[100];
int change(int y);

int main() {
	int e; //使用者要輸入幾次
	puts("please input yout wnat to input times (<100)");
	scanf("%d", &e);

	puts("please input one number");
	scanf("%d", &h[0]);



	for (int i = 0; i < e; i++) {
		printf("please input number until 10 time(leave %d times)\n", e - i);
		scanf("%d", &h[i + 1]);
		change(i + 1);

		for (int o = 0; o <= i + 1; o++) {
			printf("%d ", h[o]);
		}
	}


	system("pause");
	return 0;
}

int change(int y) {
	int s;
	for (int g = 0; g < y; g++) {
		for (int w = g + 1; w <= y; w++) {
			if (h[g] > h[w]) {
				s = h[g];
				h[g] = h[w];
				h[w] = s;
			}
		}
	}
	return 0;
}