#include<stdio.h>
#include<stdlib.h>
#include<string.h>

FILE *data = fopen("data.txt", "a+");
int i = 0, a=0, c=0, open, b = 0;
char account[20][30], passward[20][30], x[30], y[30];

int main() {
	if (data != NULL) { //����Ū����
		while (!feof(data)) {
			fscanf(data, "%s%s", account[i], passward[i]); //��Ƹ̤����n�Τ@��ťծ�}�A�Y�i����2��
			i++;
		}
	}

	open = i; //���O���@�U�{�b��i�ȬO�h�֡A�]���ڭ̥Ϊ��Oa�Ӽg��

	while (a != 3) { //�}�l�i����U�n�J���@�~
		puts("�п�J�Ʀr\n1.���U\n2.�n�J\n3.���}");
		scanf("%d", &a);
		c = 0; //��c���ȲM��

		switch (a){
		case 1:
			while (c == 0) {
				puts("�п�J�A�n���b��");
				scanf("%s", x);
				puts("�п�J�A�n���K�X");
				scanf("%s", y);
				b = 0;//�M�ŭ�

				for (int k = 0; k < i; k++) { 
					if (strcmp(account[k], x) == 0 || strcmp(passward[k], y) == 0) { //��strcmp�h����A�p2�ӬۦP�|�O0
						puts("�b���αK�X�����ơA�Э��s��J");
						b = 1;
						break;
					}
				}

				if (b != 1) {
					strcpy(account[i], x); //�νƻs�r������L�۵�
					strcpy(passward[i], y);
					c = 1;
					puts("���U���\");
				}
			}

			i++; //���@����ƼW�[�F�A�ҥH�A�[1

			break;

		case 2:
			while (c == 0) {
				puts("�п�J�A���b��");
				scanf("%s", x);
				puts("�п�J�A���K�X");
				scanf("%s", y);

				for (int k = 0; k < i; k++) { 
					if (strcmp(account[k], x) == 0 && strcmp(passward[k], y) == 0) {
						puts("�n�J���\");
						c = 1;
						break;
					}					
				}

				if (c != 1) { puts("���~�I�Э��s��J"); }
			}
			break;

		case 3:
			break;
		default:
			puts("��J���~");
			break;
		}
	}

	for (int k = open; k < i; k++) { //�N��Ƽg�J�^�h
		fprintf(data, "\n%s %s", account[k], passward[k]); //�e����\n�O�n���_�@����~��g�]���M�|�b�e�@�������ݡ^
	}

	fclose(data);

	system("pause");
	return 0;
}

