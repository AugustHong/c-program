#include<stdio.h>
#include<stdlib.h>
#include<string.h>
#include<ctype.h>

int i = 0, j=0, enter[100], n=-1, hello, m = 0; //i�O�ӷ��ɪ��Aj�O�L�o�ɪ��Aenter�O���_�檺�Ʀr�O�ĴX�ӡAn �O�Ψ��x�senter[]���O���ܼơ]�q-1�}�l���F�t�X�r��A�Ҧp��1�Ӧr�Odata[0]�A�ҥHn�o�ɤ]�O0)�Ahello�O�Ψ��x�sfgetc���ƭȡAm�O�Ψ�enter[]�}�C���x��
char data[200][30], fitler_data[100][30]; //data�O�s�i�C�@��Afitler_data�O�n�L�o���r��
int judge(char g[]), judge2(int x);

int main(int argc, char *argv[]) {

	if (argc != 3) { puts("error"); exit(1); } //�ѼƤ@�w�n3��

	FILE *source = fopen(argv[2], "r"), *fitler = fopen(argv[1], "r");

	if (fitler != NULL) { //��L�o���r��Ū�i�ɮפ�
		while (!feof(fitler)) {
			fscanf(fitler, "%s", fitler_data[j]);
			if (j == 0) { printf("# of %s�Ӧr��\n", fitler_data[j]); } // ��L�o�ɪ��L�X��
			else { printf("%s\n", fitler_data[j]); }
			j++;
		}
	}

	if (source != NULL) {
		while (!feof(source)) { //��C�@��s�idata��
			fscanf(source, "%s", data[i]); //���ۤw�|�C�@�Ӫťդ��@���A�ҥH�ۤw�|���}�]ex : this is a book�^�|�ܦ� data[0] = this , data[1] = is , data[2] = a, data[3] = book
			i++;
		}

		rewind(source); //���]��m���С]�ɮפ����^

		while ((hello = fgetc(source)) != EOF) {
			if (hello == 32) { n++; } //�p�G�O�ťժ��ܴN�n�[1�A�ҥH��1�Ӧr�O0�Ӫť�
			if (hello == 10) { n++; enter[m] = n; m++;  } //����\n������x�i�}�C���A�Bn�]�n�[1�]�]�����P��ťդ@�ˡ^(�n���[n�A�Ƕienter����)
		}
	}


	fclose(source); //Ū��������
	FILE *source2 = fopen(argv[2], "w"); //�A�}�Ӽg�J(�B�]����~�W�@������Ч˨�F�̫᭱�A�ҥH�A���}�@���ɭnŪ�J���O\n����m)�Awb+�O�iŪ�g

	for (int k = 0; k < i; k++) {  //�ݬO���O�M�L�o���r�ۦP�A�p���P�A�⥦�g�J�A�ۦP�h��X�ťդ@��
		char copy[30] = { "" };   // �C�����n���M��

		if (isalnum(data[k][strlen(data[k]) - 1]) == 0) {  //�p�G�᭱������I�Ÿ�
			for (int y = 0; y <= strlen(data[k]) - 2; y++) { copy[y] = data[k][y]; }  //�ƻs��̫�@�Ӧr���e

			if (judge(copy) == 1) { fprintf(source2, "%s", data[k]); } //�p�G�����M�L�o�r���ơA���C�L
			else { fprintf(source2, " %c", data[k][strlen(data[k]) - 1]); } //�p�G���M�L�o�r���ơA�C�L1��ťե[�W�̫�@�Ӽ��I�Ÿ�
		}
		else{
			if (judge(data[k]) == 1) { fprintf(source2, "%s", data[k]); } //�p�G�����M�L�o�r���ơA���C�L
			else { fprintf(source2, " "); } //�p�G���M�L�o�r���ơA�C�L1��ť�
		}

		if (judge2(k) == 1) { fprintf(source2, " "); } else { fprintf(source2, "\n"); }  //�p�G���O����Ÿ��h��X1��ť�
	}


	fclose(source2);
	fclose(fitler);

	system("pause");
	return 0;

}

int judge(char g[]) { //�P�_�O�_�M�L�o�r����
	int c = 1;
	for (int u = 1; u < j; u++) {
		if (strcmp(fitler_data[u], g) == 0) { c = 0; break; }
	}
	return c;
}

int judge2(int x) { //�P�_�O�_������Ÿ�
	int c = 1;
	for (int u = 0; u < m; u++) {
		if (enter[u] == x) { c = 0; break; }
	}
	return c;
}