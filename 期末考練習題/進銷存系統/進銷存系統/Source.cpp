#include<stdio.h>
#include<stdlib.h>
#include<string.h>

struct good {
	char good_name[50];
	int good_number;
	struct good *next;
};

typedef struct good Good;
typedef Good *goodlist;
int i = 0; //i�O�ΨӬݲ{�b�ӫ~���X�Ӫ�

void add_list(goodlist *head, char a[], int number, int j);
void see_list(goodlist head, int i);

int main(int argc, char *argv[]) {
	char a[50]; //a�O�Ƕi�Ӫ��ӫ~�W��
	int number, b = 0; //�Ƕi�Ӫ��ӫ~�ƶq�Ab�O�P�_�ϥΪ̿�J����H
	goodlist head = NULL;

	if (argc != 2) { puts("error"); exit(1); }

	FILE *source = fopen(argv[1], "r");

	while (!feof(source)) { //Ū�J���
		if (i == 0) { fscanf(source, "%d", &i); } //�Ĥ@�ӬO�ӫ~�ƥ�
		else{
			fscanf(source, "%s%d", a, &number);
			add_list(&head, a, number, 0); 
		}
	}

	fclose(source);

	while (b != 6 && b != 7) {
		puts("�п�J�Ʀr\n1.�i�f\n2.�P�f\n3.�d�w�s\n4.�Ҧ��w�s\n5.�x�s\n6.�x�s+���}\n7.���x�s+���}");
		scanf("%d", &b);
		switch (b) {
		case 1:
			puts("�п�J�i�f�ӫ~�W�� �ƶq");
			scanf("%s%d", a, &number);
			add_list(&head, a, number, 1);
			break;
		case 2:
			puts("�п�J�P�f�ӫ~�W�� �ƶq");
			scanf("%s%d", a, &number);
			add_list(&head, a, (-1)*number, 1);
			break;
		case 3:
			see_list(head, 3);
			break;
		case 4:
			see_list(head, 4);
			break;
		case 5:
			see_list(head, 5);
			break;
		case 6:
			see_list(head, 5);
			break;
		case 7: break;
		default: puts("��J���~"); break;
		}
	}


	system("pause");
	return 0;
}

void add_list(goodlist *head, char a[], int number, int j) {
	goodlist current, newgood;
	newgood = (goodlist)malloc(sizeof(Good));
	strcpy(newgood->good_name, a);
	newgood->good_number = number;
	newgood->next = NULL;

	if (*head == NULL) { *head = newgood; }
	else {
		current = *head;
		while (current != NULL) {
			if (strcmp(current->good_name, newgood->good_name) == 0){ //�p�G�W�٬ۦP����
				if (number >= 0) { //�p�G�O�i�f����
					current->good_number += number;
					printf("%s�{�b�٦�%d�s�f\n", current->good_name, current->good_number);
					break;
				} 
				if ((number <= 0) && ((current->good_number + number) < 0)) { puts("�s�f����"); break; } //�P�f����i�O�έt���Ӻ�A�ҥH�٬O��+
				else {
					current->good_number += number;
					printf("%s�{�b�٦�%d�s�f\n", current->good_name, current->good_number);
					break;
				}
			}

			if (current->next == NULL ) { 
				if (number > 0) { //�p�G�O�i�f�s���~
					current->next = newgood;
					if (j > 0) { //�n�MŪ�ɮɪ����ϧO
						printf("%s�{�b�٦�%d�s�f\n", newgood->good_name, newgood->good_number);
						i++;
					}
					break; //�b�̫᭱�@�_break
				}
				if (number < 0) { puts("�d�L�����~"); break; } //�p�G�p��0���ܨ��N�O�P�f�A�ӵL�����~�ҥH����P�f
				} 
			current = current->next;
		}
	}
}

void see_list(goodlist head, int j) {
	char x[50] = {"dect.txt"}; //���F���}�ɥi�H�B��A�����]�w�ϥ��|�Q���N��
	if (j == 3) { puts("�п�J�n�d�ߤ����~�W��"); scanf("%s", x); }
	if (j == 5) { puts("�п�J�n�x�s���ɦW"); scanf("%s", x); }
	FILE *source = fopen(x, "w");

	if (j == 5) { fprintf(source, "%d\n", i); } //����X�Ӱӫ~�g�J�i�h

	while (head != NULL) {
		if (j == 3 && strcmp(x, head->good_name) == 0) { printf("%s %d\n", head->good_name, head->good_number); break; } //���N���}
		if (j == 4){ printf("%s %d\n", head->good_name, head->good_number); }
		if (j == 5) { fprintf(source, "%s %d\n", head->good_name, head->good_number); }

		if (head->next == NULL && j == 3) { puts("�d�L���s�f"); } 
		head = head->next;
	}

    fclose(source); 
	if (j != 5) { remove(x); } //�R���ɮסA�]��3�M4�O�����ɮת������F�t�X5�}�ɤF
}