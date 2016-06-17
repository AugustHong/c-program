#include<stdio.h>
#include<stdlib.h>
#include<time.h>
#include<math.h>

//插入排序法(別人輸入數字即立刻排序)

int h[42], p = 1;
int mouth[] = { 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };


int main() {
	int total, number, number2;

	puts("please input yyyy-mm");
	scanf("%d-%d", &number, &number2);

	total = (number - 1) * 365 + ((number - 1) - (number - 1) % 4) / 4 - ((number - 1) - (number - 1) % 100) / 100 + ((number - 1) - (number - 1) % 400) / 400;
	//總天數(年) 其中的((number - 1) - (number - 1) % 4) / 4 - ((number - 1) - (number - 1) % 100) / 100 + ((number - 1) - (number - 1) % 400) / 400 （除4後扣除100不是，但400的是）試算有幾個閏年(直至目前)

	if (number % 4 == 0) { mouth[1] = 29; }

	for (int i = 1; i < number2; i++) { //總天數(含月不能加上自己那月)
		total += mouth[i - 1];
	}

	total += 1;//要加上1號這一天所以加上1

	for (int y = total % 7; y <= total % 7 + mouth[number2 - 1] - 1; y++) { //要加上 total % 7不然會少次數  最後檢1是因為自己重複1次 total % 7是算出1號是星期幾
		h[y] = p;
		p++;
	}

	for (int x = 0; x <= 42; x++) {
		if (x % 7 == 0) { printf("\n"); } //排版
		if (h[x] == 0) { printf("  "); }
		if (h[x] != 0) { printf("%d ", h[x]); }
		if (h[x] <= 9) { printf(" "); }
	}

	system("pause");
	return 0;
}

