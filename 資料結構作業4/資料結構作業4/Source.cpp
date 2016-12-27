#include<stdio.h>
#include<stdlib.h>
#include<string.h>

struct stu {
	char student_id[5];
	int chinese_score;
	int english_score;
	int math_score;
};

struct stu students[500];
int count = 0;

void format();
void sort(int number, int mathod);
int get_number(int number, int judge);
void print(int begin, int end, int change_number, int number);

int main() {
	int cmd = 0, mathod = 0;

	format();

	while (true) {
		printf("�п�J�H���ӱƧǡG\n1.chinese\n2.english\n3.math\n4.total_score\n5.exit\n");
		scanf("%d", &cmd);
		if (cmd == 5) { break; }
		if (cmd >= 1 && cmd <= 4) { 
			while (true) {
				printf("�п�J�ƧǤ�k�G\n1.���W\n2.����\n");
				scanf("%d", &mathod);
				if (mathod >= 1 && mathod <= 2) { sort(cmd, mathod); break; }
			}			
		}
		else { printf("your number is error\n"); }
	}

	system("pause");
	return 0;
}

void format() {
	FILE *source = fopen("scores.txt", "r");

	char a[2];
	while (!feof(source)) {
		fscanf(source, "%4s,%d,%d,%d", students[count].student_id, &students[count].chinese_score, &students[count].english_score, &students[count].math_score);
		printf("student_id: %s  total_sorce: %d\n", students[count].student_id, students[count].english_score + students[count].math_score + students[count].chinese_score);
		count += 1;
	}
	fclose(source);
}

void sort(int number, int mathod) { //number �O�n�έ��ӱƧ�  mathod�O�n���W�λ���
	struct stu change;

	for (int i = 0; i < count-1; i++) {
		for (int j = i + 1; j < count; j++) {
			if(get_number(i, number) > get_number(j, number)){
				change = students[i];
				students[i] = students[j];
				students[j] = change;
			}
		}
	}

	if (mathod == 1) { print(0, count-1, 1, number); }else { print(count-1, 0, -1, number); }
}

int get_number(int number, int judge) {
	switch (judge) {
	case 1:return students[number].chinese_score; break;
	case 2: return students[number].english_score; break;
	case 3: return students[number].math_score; break;
	case 4: return students[number].english_score + students[number].math_score + students[number].chinese_score; break;
	}
}

void print(int begin, int end, int change_number, int number) { // end + change_number�O�]���p���W���ܭn������+1�A�ӻ��W���ܭn������-1  number�O�n�έ��ӱƧ� change_number���W��1 ���-1
	char str[4][5] = { "���", "�^��", "�ƾ�", "�`��" };

	printf("�Ǹ�    %s\n", str[number-1]);
	for (int i = begin; i != end+change_number; i += change_number) { printf("%s  %d\n", students[i].student_id , get_number(i, number)); }
}