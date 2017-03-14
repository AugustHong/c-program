#include<stdio.h>
#include<stdlib.h>

int count;
int data[30];

void Merge_sort(int begin, int end);
void Merge(int begin, int mid, int end);
void deal(int a[], int number, int data_number, int current);

int main() {

	printf("請輸入總共幾筆資料：(<=30)");
	scanf("%d", &count);

	for (int i = 1; i <= count; i++) {
		printf("請輸入第%d筆資料：", i);
		scanf("%d", &data[i - 1]);
	}

	Merge_sort(0, count - 1);

	for (int i = 0; i < count; i++) { printf("%d ", data[i]); }

	system("pause");
	return 0;
}

void Merge_sort(int begin, int end) {
	int mid;

	if (begin < end) { //如果要求的陣列中，end > begin 才執行（所以相等是不執行的）
		mid = (end + begin) / 2;
		Merge_sort(begin, mid); //切成2部份再做Merge_sort(左部份）
		Merge_sort(mid + 1, end); //切成2部份再做Merge_sort(右部份）
		Merge(begin, mid, end); //把2部份合併起來
	}
}

void Merge(int begin, int mid, int end) {
	int n1 = mid - begin + 1; //左區塊個數
	int n2 = end - mid; //右區塊個數

	int left[15], right[15];

	for (int i = 0; i < n1; i++) { left[i] = data[begin + i]; } //左陣列
	for (int i = 0; i < n2; i++) { right[i] = data[mid + i + 1]; } //右陣列

	int k = 0, l = 0;

	for (int i = begin; i <= end; i++) { //左和右比較
		if (left[k] <= right[l]) {
			data[i] = left[k];
			k += 1;
			if (k == n1) { deal(right, l, end - i, i+1); return; } //如果到底邊了，把另一陣列的其他值直接加進去data
		}
		else {
			data[i] = right[l];
			l += 1;
			if (l == n2) { deal(left, k, end - i, i+1); return; } //如果到底邊了，把另一陣列的其他值直接加進去data
		}
	}
}

void deal(int a[], int number, int data_number, int current) {
	for (int i = number; i < number + data_number; i++) { data[current] = a[i]; current += 1; }
}