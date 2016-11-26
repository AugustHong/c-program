#include<stdio.h>
#include<stdlib.h>
#include<time.h>

int card[54], user[26], computer[26];
int user_current_number = 0, computer_current_number = 0;
int input_number;

void format();
void start();
void judge(int number, int &i, int id[]);
void move(int &i, int id[], int same);
void print();
void rnd();

int main() {

	format();
	start();

	system("pause");
	return 0;
}

void format() {
	int change, a;

	for (int i = 1; i <= 13; i++) {  //����card�̭�����
		for (int j = (i-1)*4; j <= (i-1)*4 + 3; j++) { card[j] = i; }
	}

	card[52] = 14;
	card[53] = 14;

	srand(time(NULL));
	for (int i = 53; i >= 0; i--) { //�C�����]��ƶi�h
		a = rand() % (i+1);
		if (i % 2 == 1) { judge(card[a], user_current_number, user); }
		else { judge(card[a], computer_current_number, computer); }

		change = card[a];  //�A���洫
		card[a] = card[i];
		card[i] = change;
	}
}

void start() {
	int a, result = 0;
	srand(time(NULL));
	while (user_current_number != 0 && computer_current_number != 0) {
		print();
		scanf("%d", &input_number);
		if (input_number < 0 || input_number >= computer_current_number) { printf("��J �W�X�d��I�Э��s��J\n"); }
		else {
			printf("�A��F�L��%d\n", computer[input_number]);
			judge(computer[input_number], user_current_number, user); //�ϥΪ̩�P�A�ݬO�_�[�J�٬O����
			move(computer_current_number, computer, input_number); //�٭n���P�ծ����]�]�Q�⨫�F�^

			if (computer_current_number == 0) { printf("you lost"); result = 1; } //�b���ɴN�n�P�_�F�A���M�|�X����I
			if (user_current_number == 0) { printf("you win"); result = 1; }

			if (result == 0) { //�p�G�X�ӵ��G�F�N���n�A�]�F
				a = rand() % (user_current_number); //�]��user_current_number���ӴN���}�C�h1�A�ҥH���ΦA�h+1�F
				printf("�L��F�A��%d\n", user[a]);
				judge(user[a], computer_current_number, computer);
				move(user_current_number, user, a);	
				rnd();
			}
		}
	}
}

void judge(int number, int &i, int id[]) {
	int c = 0, same = 0;

	for (int j = 0; j < i; j++) { if (number == id[j] && number != 14) { c = 1; same = j; } } //�p�G���O���P�S�ۦP�A�h��c=1

	if (c == 0) { id[i] = number; i += 1; }else {  move(i, id, same); } //�ۦP���ܴN��move()�A�p�G�S���h�[�J
}

void move(int &i, int id[], int same) {
	id[same] = 0; //�����ƪ�������1

	for (int j = same; j < i; j++) { id[j] = id[j + 1]; } //�������e���@��

	i -= 1;
}

void print() {
	printf("user: ");
	for (int i = 0; i < user_current_number; i++) {
		if (user[i] != 0) { printf("%d ", user[i]); }
	}

	printf("\n");
	
	printf("computer: ");
	for (int i = 0; i < computer_current_number; i++) {
		if (computer[i] != 0) { printf("X "); }
	}
	printf("�п�X�Ʀr�ӿ�P(�q0�}�l�ƨ�%d)\n",computer_current_number-1);
}

void rnd() { //�q���n�~�P
	int a, b, change;
	srand(time(NULL));
	for (int i = 0; i < 10; i++) {
		a = rand() % (computer_current_number);
		b = rand() % (computer_current_number);

		change = computer[a];
		computer[a] = computer[b];
		computer[b] = change;
	}
}