#include<stdio.h>
#include<stdlib.h>
#include<string.h>
#include<iostream>
#include<math.h>

class  Rational {
private:
	int numerator; // 分子
	int denominator; // 分母
	int i, j, z; //用來做暫時處理存取之用(i是分子，j是分母)，z是用來存他們的最大公因數的
public:
	void setting(int x, int y) { numerator = x;  denominator = y; } //認置第1筆

	int max(int x, int y){//最大公因數(展轉相除法)
		while (x != y) {
			if (x > y) { x -= y; } else { y -= x; }
		}
		return x;
	} 

	void add(int x, int y){  //加
		i = numerator * y + x * denominator;
		j = denominator * y;
		if (max(fabs(i), fabs(j)) != 1) { z = max(fabs(i), fabs(j));  i /= z; j /= z; } //如果最大公因數不是0那就讓他除他的最大公因數
		printf("%d\n", i); puts("--"); printf("%d\n", j);  //印出來
		setting(i, j); //讓他的值回到那
	} 

	void minus(int x, int y){// 減
		i = numerator * y - x * denominator;
		j = denominator * y;
		if (max(fabs(i), fabs(j)) != 1) { z = max(fabs(i), fabs(j));  i /= z; j /= z; } //如果最大公因數不是0那就讓他除他的最大公因數
		printf("%d\n", i); puts("--"); printf("%d\n", j);  //印出來
		setting(i, j); //讓他的值回到那
	} 

	void multiply(int x, int y){//乘
		i = numerator * x;
		j = denominator * y;
		if (max(fabs(i), fabs(j)) != 1) { z = max(fabs(i), fabs(j));  i /= z; j /= z; } //如果最大公因數不是0那就讓他除他的最大公因數
		printf("%d\n", i); puts("--"); printf("%d\n", j);  //印出來
		setting(i, j); //讓他的值回到那
	} 

	void divided(int x, int y){//除
		i = numerator * y;
		j = denominator * x;
		if (max(fabs(i), fabs(j)) != 1) { z = max(fabs(i), fabs(j));  i /= z; j /= z; } //如果最大公因數不是0那就讓他除他的最大公因數
		printf("%d\n", i); puts("--"); printf("%d\n", j);  //印出來
		setting(i, j); //讓他的值回到那
	} 

};

int main() {
	Rational  number;
	int a, b, c=0; //暫時存的資料，c是用來判斷的
	puts("請輸入第1筆資料(分子 分母)");
	scanf("%d%d", &a, &b);

	number.setting(a, b);


	while (c != 5) {
		puts("請輸入\n1.加\n2.減\n3.乘\n4.除\n5.離開");
		scanf("%d", &c);
		switch (c) {
		case 1:
			puts("請輸入第2筆資料(分子 分母)");
			scanf("%d%d", &a, &b);
			number.add(a, b);
			break;
		case 2:
			puts("請輸入第2筆資料(分子 分母)");
			scanf("%d%d", &a, &b);
			number.minus(a, b);
			break;
		case 3:
			puts("請輸入第2筆資料(分子 分母)");
			scanf("%d%d", &a, &b);
			number.multiply(a, b);
			break;
		case 4:
			puts("請輸入第2筆資料(分子 分母)");
			scanf("%d%d", &a, &b);
			number.divided(a, b);
			break;
		case 5:
			break;
		default:
			puts("輸入錯誤請重新輸入");
			break;
		}
	}


	system("pause");
	return 0;
}