#include<stdio.h>
#include<stdlib.h>
#include<string.h>

char input_str[10], result[10];
int max;

char* change(char *str, int i);
void start(char *str, char *result, int max);

int main() {
	printf("請輸入不重複的字串");
	scanf("%s", input_str);
	printf("請輸出最多幾個字元（例如：3，且一定要小於等於您輸出的字元長度內）");
	scanf("%d", &max);
	result[max] = '\0';

	start(input_str, result, max);

	system("pause");
	return 0;
}

char* change(char *str, int i) {
	char change[10];
	strcpy(change, str);

	for (int j = i; change[j] != '\0'; j++) { //把被拿出來的字元刪掉（變成沒那字元的字串）
		change[j] = change[j + 1];
	}
	return change;
}

void start(char* str, char* result, int number) {
	char a[10];

	if (number == 0) { printf("%s\n", result); }
	else {
		for (int i = 0; i < strlen(str); i++) { //把它的字元一個一個弄進去
			result[max - number] = str[i];
			strcpy(a, change(str, i)); //一定要讓它存到一字串中，不然會錯
			start(a, result, number - 1); //慢慢把它填完空格（遞迴）
		}
	}
}