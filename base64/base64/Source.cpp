#include<stdio.h>
#include<stdlib.h>
#include<string.h>

int c[25], y, k;
char abc[4];
char judge[65] = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+-";
void change(int x, int i);
int pow(int x, int y), change2(int x);



int main() {
	puts("please input one number and one string, and use one space to split it ");
	scanf("%d%s", &y, abc);

	for (int g = 0; g < y; g++) { change(int(abc[g]), g * 8); } //有8個數字，所以要用*8

	for (int r = 0; r <= y; r++) {
		k = 0;
		k += change2(r * 6); //因為取6個出來算，所以用*6
		if (k > 65) { printf("="); }
		else { printf("%c", judge[k]); }
	}

	for (int u = y+2; u <= 4; u++) { printf("="); } //補足4個位數


	system("pause");
	return 0;
}


void change(int x, int i) { // change to 2進位
	for (int j = 7; j >= 0; j--) {
		if (pow(2, j) > x) { c[i] = 0; }
		else {
			c[i] = x / pow(2, j);
			x = x % pow(2, j);
		}
		i++;
	}
}

int pow(int x, int y) { //算出來次方
	int total = 1;
	for (int i = 0; i < y; i++) { total *= x; }
	return total;
}

int change2(int x) { //轉回來(2 的0次方到5次方)
	int total = 0;
	for (int g = 5; g >= 0; g--) { total += c[5 - g + x] * pow(2, g); }
	return total;
}