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

	for (int i = 1; i <= 13; i++) {  //先讓card裡面有值
		for (int j = (i-1)*4; j <= (i-1)*4 + 3; j++) { card[j] = i; }
	}

	card[52] = 14;
	card[53] = 14;

	srand(time(NULL));
	for (int i = 53; i >= 0; i--) { //每次都跑資料進去
		a = rand() % (i+1);
		if (i % 2 == 1) { judge(card[a], user_current_number, user); }
		else { judge(card[a], computer_current_number, computer); }

		change = card[a];  //再做交換
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
		if (input_number < 0 || input_number >= computer_current_number) { printf("輸入 超出範圍！請重新輸入\n"); }
		else {
			printf("你抽了他的%d\n", computer[input_number]);
			judge(computer[input_number], user_current_number, user); //使用者抽牌，看是否加入還是拿掉
			move(computer_current_number, computer, input_number); //還要讓牌組消掉（因被抽走了）

			if (computer_current_number == 0) { printf("you lost"); result = 1; } //在此時就要判斷了，不然會出錯喔！
			if (user_current_number == 0) { printf("you win"); result = 1; }

			if (result == 0) { //如果出來結果了就不要再跑了
				a = rand() % (user_current_number); //因為user_current_number本來就比原陣列多1，所以不用再多+1了
				printf("他抽了你的%d\n", user[a]);
				judge(user[a], computer_current_number, computer);
				move(user_current_number, user, a);	
				rnd();
			}
		}
	}
}

void judge(int number, int &i, int id[]) {
	int c = 0, same = 0;

	for (int j = 0; j < i; j++) { if (number == id[j] && number != 14) { c = 1; same = j; } } //如果不是鬼牌又相同，則讓c=1

	if (c == 0) { id[i] = number; i += 1; }else {  move(i, id, same); } //相同的話就到move()，如果沒有則加入
}

void move(int &i, int id[], int same) {
	id[same] = 0; //讓重複的那筆為1

	for (int j = same; j < i; j++) { id[j] = id[j + 1]; } //全部往前移一格

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
	printf("請輸出數字來選牌(從0開始數到%d)\n",computer_current_number-1);
}

void rnd() { //電腦要洗牌
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