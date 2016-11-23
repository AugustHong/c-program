#include<stdio.h>
#include<stdlib.h>

#define max 10

int data[max][10] = { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2,   //邊界是2 黑棋是-1 白棋是1 空格是0
					 2, 1, 1, 1, 0, 0, 0, 0, 0, 2,
					 2, 1, -1, 1, 0, 0, 0, 0, 0, 2,
					 2, 1, -1, 1, 0, 0, 0, 0, 0, 2,
					 2, 1, 0, 1, 0, 0, 1, 0, 0, 2,
					 2, 0, 0, 0, 0, 1, -1, 1, 0, 2,
					 2, 0, 0, 0, 1, -1, -1, -1, 1, 2,
					 2, 0, 0, 0, 0, 1, -1, 0, 0, 2,
					 2, 0, 0, 0, 0, 0, 0, 0, 0, 2,
					 2, 2, 2, 2, 2, 2, 2, 2, 2, 2 };

int record[30][2];
int current = 0, a, b;

void print();
bool exit(int x, int y);
bool judge(int x, int y, int color);
void clear(bool a);

int main() {
	puts("測試值(4,2) (8,6)");
	print();

	printf("請輸入2個數字\n");
	scanf("%d %d", &a, &b);
	data[a][b] = 1;

	clear(judge(a + 1, b, -1));
	clear(judge(a - 1, b, -1));
	clear(judge(a, b + 1, -1));
	clear(judge(a, b - 1, -1));

	print();

	system("pause");
	return 0;
}

void print() {
	puts("   0  1  2  3  4  5  6  7  8  9");
	for (int i = 0; i < max; i++) {
		printf("%d  ", i);
		for (int j = 0; j < max; j++) {
			if (data[i][j] == 0) { printf("空 "); }
			if (data[i][j] == 2) { printf("邊 "); }
			if (data[i][j] == 1) { printf("白 "); }
			if (data[i][j] == -1) { printf("黑 "); }
		}
		printf("\n");
	}
}

bool exit(int x, int y) {
	for (int i = 0; i < 30; i++) {
		if (record[i][0] == x && record[i][1] == y) { return false; }
	}

	return true;
}

void clear(bool a) { //把全部都清除

	if (a){
		for (int i = 0; i <= current; i++){ record[i][0] = 0; record[i][1] = 0;} //就算找到出口還是要讓record的清空
	}
	else {
		for (int i = 0; i <= current; i++) {
			if (record[i][0] != 0 && record[i][1] != 0) {
				data[record[i][0]][record[i][1]] = 0;
				printf("已清除(%d, %d)\n", record[i][0], record[i][1]);
				record[i][0] = 0;
				record[i][1] = 0;
			}
		}
	}
	current = 0; //clear current
}

bool judge(int x, int y, int color) {

	if (data[x + 1][y] == 0 || data[x - 1][y] == 0 || data[x][y + 1] == 0 || data[x][y - 1] == 0) { return true; }

	printf("找(%d, %d)\n", x, y);
	record[current][0] = x;
	record[current][1] = y;
	current += 1;

	if (data[x + 1][y] == color && exit(x + 1, y)) { if (judge(x + 1, y, color)) { return true; }; }  //要同色且不能是找過的(如果已找到可以出去了！那就不用再找了)，就維持這樣一直找到有出回或沒出回為止
	if (data[x - 1][y] == color && exit(x - 1, y)) { if ( judge(x - 1, y, color)) { return true; }; }
	if (data[x][y + 1] == color && exit(x, y + 1)) {if (judge(x, y + 1, color)) { return true; };}
	if (data[x][y - 1] == color && exit(x, y - 1)) { if (judge(x, y - 1, color)) { return true; }; }

	return false;
}

