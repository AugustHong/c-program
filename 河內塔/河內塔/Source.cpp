#include<stdio.h>
#include<stdlib.h>

int start(int num, char source, char goal, char butter);

int main() {
	int input;
	int input2, input3, input4;

	puts("請輸入河內塔的盤子數和來源塔和目標塔及第3座塔的名稱（請各用1格空白格空）");
	scanf("%d %c %c %c", &input, &input2, &input3, &input4);
	start(input, input2, input3, input4);

	system("pause");
	return 0;
}

int start(int num, char source, char goal, char butter) {
	if (num == 1) { printf("%d dick from %c move to %c\n", num, source, goal); } //移動1號盤子
	if (num > 1) {
		start(num - 1, source, butter, goal);//將其他的先用到butter區
		printf("%d dick from %c move to %c\n", num, source, goal);//移動最後一個盤子到goal
		start(num - 1, butter, goal, source);//將butter區的用到goal
	}
	return 0;
}