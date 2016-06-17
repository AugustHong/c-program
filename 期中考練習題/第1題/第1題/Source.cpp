#include<stdio.h>
#include<stdlib.h>

void date(int y, int m, int d, int x);
int data[20][4];

int main() {
	int number;
	char *kk[] = { "日","一","二","三","四","五","六" };

	puts("你要輸入幾個日期");
	scanf("%d", &number);

	for (int i = 0; i < number; i++) {
		puts("請輸入西元年月日(格式為yyyy-mm-dd)");
		scanf("%d-%d-%d", &data[i][0], &data[i][1], &data[i][2]);
		date(data[i][0], data[i][1], data[i][2], i);
	}

	for (int j = 0; j <= 6; j++) {
		for (int i = 0; i < number; i++) {
			if (data[i][3] == j) {
				printf("%d年%d月%d日星期%s\n", data[i][0], data[i][1], data[i][2], kk[j]);
			}
		}
	}

	system("pause");
	return 0;
}

void date(int y, int m, int d, int x) {
	int total = 0;
	int mouth[] = { 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };

	total = (y - 1) * 365 + ((y- 1) - (y - 1) % 4) / 4 - ((y - 1) - (y - 1) %100) / 100 + ((y - 1) - (y - 1) % 400) / 400; 
	//總天數(年) 其中的((y- 1) - (y - 1) % 4) / 4 - ((y - 1) - (y - 1) %100) / 100 + ((y - 1) - (y - 1) % 400) / 400 （除4後扣除100不是，但400的是）試算有幾個閏年(直至目前)

	if (y % 4 == 0) { mouth[1] = 29; }

	for (int i = 1; i < m; i++) { //總天數(含月不能加上自己那月)
		total += mouth[i - 1];
	}

	total += d ; //日期（含自己那天）

	data[x][3] = total % 7;
}