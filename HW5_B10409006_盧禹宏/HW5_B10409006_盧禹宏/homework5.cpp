#include<stdio.h>
#include<stdlib.h>
#include<string.h>
#include<ctype.h>


int main(int argc, char *argv[]) {
	char data; //data�O�ΨӦs���U�Ӫ��C�Ӧr����
	int number, c = 0, change = 0; //number�O�Ψӧ�L�ܦ�ascii���Q�i��Ac�O�ΨӧP�_%�᪺2��ҥH�Ϊ��Achange�O�Ψ��x�s�n�ন10�i�쪺16�i��

	if (argc != 4) { puts("error"); exit(1); } // �p�G�����ѼƤӤ֫h�����檽�����}

	FILE *source = fopen(argv[2], "r"), *dect = fopen(argv[3], "w");

	if (source != NULL) {
		switch (argv[1][0]) {
		case 'e':
		case 'E':
			while ((number = fgetc(source)) != EOF) {
				data = number; // �]��data�O�r�����A�A�ҥH�|�ۤw�ഫ���r��

				if (number != 10 ) {  //����Ÿ��n���L����A��X�]�n��\n�O10�^
					if (isalnum(number)) { fprintf(dect, "%c", data); } //�p�G�O�^��A���i�H������ and �p�G�O�Q�i��Ʀr�A�h�]�i�H������
					else {   // �p�G�O��L���ܡA���N��L���Q�i���ܦ�16�i��h�g�J�]%x�O�i�H������Q�i���ܦ�16�i��^
						fprintf(dect, "%%");
						fprintf(dect, "%x", number);
					}
				}
				else { fprintf(dect, "\n"); }

			}

			fclose(source); //�����ɮ�
			fclose(dect);

			break;


		case 'd': //���ǫܭ��n�]�@�w�n������L������Ac == 2 �Ac == 1�Ac == 0���M�|�����@������U�h�^
		case 'D':
			while ((number = fgetc(source)) != EOF) {
				data = number;

				if (number != 10) {

					if (c == 0 && data != '%') { fprintf(dect, "%c", data); } //��L���ҥH�����L��ؼФW

					if (c == 2) { //%�᪺��2�Ӧ�ơA�ҥH�ন10�i��n�[�W�h�]�òM��c �Mchange�^
						if (int(data) >= 48 && int(data) <= 57) { change += (int(data) - 48); }
						if (int(data) >= 97 && int(data) <= 102) { change += (int(data) - 87); }

						fprintf(dect, "%c", change);
						change = 0;
						c = 0;
					}

					if (c == 1) { //%�᪺��1�Ӧ�ơA�ҥH�n�ন10�i��n���W16
						if (int(data) >= 48 && int(data) <= 57) { change += (int(data) - 48) * 16; }
						if (int(data) >= 97 && int(data) <= 102) { change += (int(data) - 87) * 16; } // �|��a ��f�ҥH�n�S�O���P�_
						c = 2;
					}

					if (c == 0 && data == '%') { c = 1; } //�J��%�ҥH��c�ܦ�1
				}
				else { fprintf(dect, "\n"); }
			}

			fclose(source);
			fclose(dect);

			break;

		}
	}

	system("pause");
	return 0;
}