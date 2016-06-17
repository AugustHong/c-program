//第三題（遞迴與非遞迴的比較）

#include<stdio.h>
#include<stdlib.h>
#include<time.h>

int Fibonacci(int i); //宣告函式

int main() {
	int input, a0 = 1, a1 = 2, a2 = 2, result;
	clock_t start, end;

	printf("請輸入正整數:\n");
	scanf("%d", &input);
	printf("%d\n", Fibonacci(input));

	//這是執行有遞迴的100萬次（start, end是代表時間的開始和結束）
	start = clock();
	for (int i = 1; i <= 1000000; i++){
		Fibonacci(input);
	}
	end = clock();
	printf("遞迴:%f sec\n", (double)(end - start) / CLOCKS_PER_SEC); //要用double的型態才行

	//這是非遞迴的100萬次
	start = clock();
	for (int k = 1; k <= 1000000; k++) { //作100萬次
		a0 = 1;  //重置初使值（不然跑100萬次時都不是從一開始跑）
		a1 = 2;
		a2 = 2;

		for (int g = 1; g <= input; g++){  //求input的值
			result = 2 * a2 + a1 - a0; //求出值
			//交換（把值都互換，以便接下來的運算）
			a0 = a1;
			a1 = a2;
			a2 = result;
		}
	}
	end = clock();
	printf("非遞迴:%f sec\n", (double)(end - start) / CLOCKS_PER_SEC);

	system("pause");
}


int Fibonacci(int i) { //遞迴的函式
	if (i == 0) { return 1; }
	if (i == 1 || i == 2) { return 2; }
	if (i >= 3) { return  2 * Fibonacci(i - 1) + Fibonacci(i - 2) - Fibonacci(i - 3); }
}