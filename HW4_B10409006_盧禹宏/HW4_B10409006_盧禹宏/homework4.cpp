//第四題（組合排列類似題目）
#include<stdio.h>
#include<stdlib.h>
#include<math.h>

int Permutations(int n, int m);
int Judge(int y, int m);
int h[100]; //改成全域變數

int main() {
	int n, m;
	printf("請輸入n 和 m(用一格空白隔開，且請n 大於m)");
	scanf("%d %d", &n, &m);

	for (int i = 1; i <= m; i++) {  //先給一開始的值（如1,2,3……等）
		h[i - 1] = i;
	}

	Permutations(n, m);

	system("pause");
}

int Permutations(int n, int m) {

	for (int i = 0; i <= 10000000; i++) {

		if (h[0] > n) { break; }//如果第1項大於n，就離開

		if (Judge(1, m) == 1){ //每次都讓它進去1，如果回傳出來是false的話那就是0
			for (int j = 0; j <= (m - 1); j++) {  //輸出資料（必需小於項數，且要沒有重複的才行（c == 1 是沒有重複的））
				printf("%d ", h[j]);
			}

			printf("\n"); //空1行
		}

		
		
		h[m - 1] += 1; //讓值加1（都是最後一項在加）

		for (int g = m - 1; g >= 1; g--) {  //如果值大於n話，自己變成1，前1項加1（就像(2,2)中一開始是1,2但是2+1=3超過2了，所以就前一項加1自已變1，就會變成2,1了）
			if (h[g] > n) {
				h[g - 1] += 1;
				h[g] = 1;
			}
		}

	}

	return n, m; //要回傳值
}

int Judge(int y, int m) { //看是否有重複的
	for (int k = 0; k <= (m - 1) - 1; k++) {
		for (int p = k + 1; p <= (m - 1); p++) {
			if (h[k] == h[p] ) { 
				y = 0;
				break;
			}
		}
		if (y == 0) { break; } //加速離開（免得跑了無意義的迴圈）
	}

	return y;
}