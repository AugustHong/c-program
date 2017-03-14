#include<stdio.h>
#include<stdlib.h>

int count;
int data[30];

void Merge_sort(int begin, int end);
void Merge(int begin, int mid, int end);
void deal(int a[], int number, int data_number, int current);

int main() {

	printf("�п�J�`�@�X����ơG(<=30)");
	scanf("%d", &count);

	for (int i = 1; i <= count; i++) {
		printf("�п�J��%d����ơG", i);
		scanf("%d", &data[i - 1]);
	}

	Merge_sort(0, count - 1);

	for (int i = 0; i < count; i++) { printf("%d ", data[i]); }

	system("pause");
	return 0;
}

void Merge_sort(int begin, int end) {
	int mid;

	if (begin < end) { //�p�G�n�D���}�C���Aend > begin �~����]�ҥH�۵��O�����檺�^
		mid = (end + begin) / 2;
		Merge_sort(begin, mid); //����2�����A��Merge_sort(�������^
		Merge_sort(mid + 1, end); //����2�����A��Merge_sort(�k�����^
		Merge(begin, mid, end); //��2�����X�ְ_��
	}
}

void Merge(int begin, int mid, int end) {
	int n1 = mid - begin + 1; //���϶��Ӽ�
	int n2 = end - mid; //�k�϶��Ӽ�

	int left[15], right[15];

	for (int i = 0; i < n1; i++) { left[i] = data[begin + i]; } //���}�C
	for (int i = 0; i < n2; i++) { right[i] = data[mid + i + 1]; } //�k�}�C

	int k = 0, l = 0;

	for (int i = begin; i <= end; i++) { //���M�k���
		if (left[k] <= right[l]) {
			data[i] = left[k];
			k += 1;
			if (k == n1) { deal(right, l, end - i, i+1); return; } //�p�G�쩳��F�A��t�@�}�C����L�Ȫ����[�i�hdata
		}
		else {
			data[i] = right[l];
			l += 1;
			if (l == n2) { deal(left, k, end - i, i+1); return; } //�p�G�쩳��F�A��t�@�}�C����L�Ȫ����[�i�hdata
		}
	}
}

void deal(int a[], int number, int data_number, int current) {
	for (int i = number; i < number + data_number; i++) { data[current] = a[i]; current += 1; }
}