#include<stdio.h>
#include<stdlib.h>

void clear();
int judge(int x, int y);

int number[17][17], judge_num[17] = { 0 }; //number是來存資料的，而judge_number是用來判斷是否有重複的
int c = -1; //c是用來當作判斷輸出用的(0是錯的，1是對的)

int main(int argc, char *argv[]) {	
	int i = 1, j = 1; //j是行i是列

	if (argc != 2) { puts("error"); exit(1); }
	FILE *source = fopen(argv[1], "r");

	while (!feof(source)) {
		fscanf(source, "%d", &number[j][i]);
		i++;
		if (i > 16) { i = 1; j++; } //如果超過16則讓二維陣列的頭+1，自已則變成1
	}

	for (int x = 1; x <= 16; x++) { //一排一排找
		for (int y = 1; y <= 16; y++) {
			judge_num[number[x][y]] ++; //讓數字自己加1，如果有重複則會是2
			if (judge_num[number[x][y]] == 2) {c = 0; break; } //重複了！
		}
		clear(); //跑完一筆就要清掉，不然會錯
		if (c == 0) { break; }
	}


	if (c != 0) {
		for (int x = 1; x <= 16; x++) { //一列一列找
			for (int y = 1; y <= 16; y++) {
				judge_num[number[y][x]] ++; //讓數字自己加1，如果有重複則會是2
				if (judge_num[number[y][x]] == 2) { c = 0; break; } //重複了！
			}
			clear(); //跑完一筆就要清掉，不然會錯
			if (c == 0) { break; }
		}
	}


	if (c != 0) {
		for (int x = 1; x <= 13; x += 4) { //一塊一塊去找
			for (int y = 1; y <= 13; y += 4) {if (judge(x, y) == 0) { c = 0;  break; }}
			if (c == 0) { break; }
		}
	}

	if (c == 0) { puts("False"); }else { puts("True"); } //輸出

	system("pause");
	return 0;
}

void clear() { //清回值，不然會錯
	for (int i = 1; i <= 16; i++) { judge_num[i] = 0; }
}

int judge(int x, int y) {
	clear(); //跑完一筆就要清掉，不然會錯

	for (int i = x; i < x+4; i++) {
		for (int j = y; j < y+4; j++) {
			judge_num[number[i][j]] ++; //讓數字自己加1，如果有重複則會是2
			if (judge_num[number[i][j]] == 2) { return 0; } //重複了！
		}
	}
	return 1;
}