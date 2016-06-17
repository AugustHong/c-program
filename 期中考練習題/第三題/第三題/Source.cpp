#include<stdio.h>
#include<stdlib.h>
#include<math.h>

int number[100], ok[10000], n, a=0; //n是有幾項，a是有幾個值要存進去（會累加）
int judge(int x), val();
void change(int x, int y), start(int x);

int main() {

	puts("請輸入n(n是表是有幾個數字要排)");
	scanf("%d", &n);

	for (int i = 0; i < n; i++) { //將值放入裡面
		puts("請輸入要排的數值");
		scanf("%d", &number[i]);
	}

	start(0);

	system("pause");
	return 0;
}


int val() {
	int total = 0;
	for (int t = n-1; t >= 0; t--) { total += number[n-1 - t] * pow(10, t); } //把他算成數用(3,2,1) =3*100+2*10+1*1
	return total;
}

int judge(int x) {
	int c = 1;

	for (int r = 0; r < x; r++) {if (val() == ok[r]) { c = 0; }}

	return c;
}

void change(int x, int y) {
	int w;
	w = number[x];
	number[x] = number[y];
	number[y] = w;
}

void start(int x) {
	if (x == n-1) {
		if (judge(a) == 1) {
			for (int g = 0; g < n; g++) { printf("%d", number[g]); }
			printf("\n");

			ok[a] = val();
			a++;
		}
	}
	else {
		for (int i = x; i < n; i++) {
			change(i, x);
			start(x + 1);
			change(i, x); //把交換的數再交換回來
		}
	}
}
