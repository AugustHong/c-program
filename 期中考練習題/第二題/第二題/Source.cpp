#include<stdio.h>
#include<stdlib.h>

int number[50], i=1, a , b, c;//a 是用來存特別號的，c是用來算有幾個中的，b和a一樣是暫存的，但是沒有特別要存什麼
void judge(int s, int r);

int main() {
	while (i < 8) { 
		puts("請輸入開獎的號碼(第7個號碼是特別號喔！)");
		scanf("%d", &a);
		judge(a, 1);		
	}

	while (i < 14) { 
		puts("請輸入您的號碼");
		scanf("%d", &b);
		judge(b, 2);
	}

	for (int j = 1; j <= 49; j++) {
		if (j != a && number[j] == 2) { c += 1; }
	}

	if (number[a] == 2) { c += 10; }// 如果有中特別號就讓數字+10來分別

	switch (c) {
	case 6:
		printf("頭獎");
		break;
	case 15:
		printf("二獎");
		break;
	case 5:
		printf("三獎");
		break;
	case 14:
		printf("四獎");
		break;
	case 4:
		printf("五獎");
		break;
	case 13:
		printf("六獎");
		break;
	case 12:
		printf("七獎");
		break;
	case 3:
		printf("普獎");
		break;
	default:
		printf("沒中獎");
		break;
	}

	system("pause");
	return 0;

}

void judge(int s, int r) {
	if (number[s] == r || s > 49 || s < 1) { puts("請重新輸入"); }
	else {
		number[s] += 1;
		i++;
	}
}