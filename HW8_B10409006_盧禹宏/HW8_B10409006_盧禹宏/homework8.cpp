#include<stdio.h>
#include<stdlib.h>
#include<string.h>
#include<iostream>
#include<math.h>

class  Rational {
private:
	int numerator; // ���l
	int denominator; // ����
	int i, j, z; //�ΨӰ��ȮɳB�z�s������(i�O���l�Aj�O����)�Az�O�ΨӦs�L�̪��̤j���]�ƪ�
public:
	void setting(int x, int y) { numerator = x;  denominator = y; } //�{�m��1��

	int max(int x, int y){//�̤j���]��(�i��۰��k)
		while (x != y) {
			if (x > y) { x -= y; } else { y -= x; }
		}
		return x;
	} 

	void add(int x, int y){  //�[
		i = numerator * y + x * denominator;
		j = denominator * y;
		if (max(fabs(i), fabs(j)) != 1) { z = max(fabs(i), fabs(j));  i /= z; j /= z; } //�p�G�̤j���]�Ƥ��O0���N���L���L���̤j���]��
		printf("%d\n", i); puts("--"); printf("%d\n", j);  //�L�X��
		setting(i, j); //���L���Ȧ^�쨺
	} 

	void minus(int x, int y){// ��
		i = numerator * y - x * denominator;
		j = denominator * y;
		if (max(fabs(i), fabs(j)) != 1) { z = max(fabs(i), fabs(j));  i /= z; j /= z; } //�p�G�̤j���]�Ƥ��O0���N���L���L���̤j���]��
		printf("%d\n", i); puts("--"); printf("%d\n", j);  //�L�X��
		setting(i, j); //���L���Ȧ^�쨺
	} 

	void multiply(int x, int y){//��
		i = numerator * x;
		j = denominator * y;
		if (max(fabs(i), fabs(j)) != 1) { z = max(fabs(i), fabs(j));  i /= z; j /= z; } //�p�G�̤j���]�Ƥ��O0���N���L���L���̤j���]��
		printf("%d\n", i); puts("--"); printf("%d\n", j);  //�L�X��
		setting(i, j); //���L���Ȧ^�쨺
	} 

	void divided(int x, int y){//��
		i = numerator * y;
		j = denominator * x;
		if (max(fabs(i), fabs(j)) != 1) { z = max(fabs(i), fabs(j));  i /= z; j /= z; } //�p�G�̤j���]�Ƥ��O0���N���L���L���̤j���]��
		printf("%d\n", i); puts("--"); printf("%d\n", j);  //�L�X��
		setting(i, j); //���L���Ȧ^�쨺
	} 

};

int main() {
	Rational  number;
	int a, b, c=0; //�Ȯɦs����ơAc�O�ΨӧP�_��
	puts("�п�J��1�����(���l ����)");
	scanf("%d%d", &a, &b);

	number.setting(a, b);


	while (c != 5) {
		puts("�п�J\n1.�[\n2.��\n3.��\n4.��\n5.���}");
		scanf("%d", &c);
		switch (c) {
		case 1:
			puts("�п�J��2�����(���l ����)");
			scanf("%d%d", &a, &b);
			number.add(a, b);
			break;
		case 2:
			puts("�п�J��2�����(���l ����)");
			scanf("%d%d", &a, &b);
			number.minus(a, b);
			break;
		case 3:
			puts("�п�J��2�����(���l ����)");
			scanf("%d%d", &a, &b);
			number.multiply(a, b);
			break;
		case 4:
			puts("�п�J��2�����(���l ����)");
			scanf("%d%d", &a, &b);
			number.divided(a, b);
			break;
		case 5:
			break;
		default:
			puts("��J���~�Э��s��J");
			break;
		}
	}


	system("pause");
	return 0;
}