#include<stdio.h>
#include<stdlib.h>
#include<time.h>
#include<conio.h> //_kbhit �M _getch �����Y��

struct tm *tm1; // �ɶ����@�ӵ��c

int main() {
	time_t time1; //�ŧi�ɶ��ܼ�

	while (true) {

		if (_kbhit() && _getch() == 'q') { break; } //_kbhit�O�ϥΪ̦��L��J��ơA�p�G���Otrue�S���hfalse
		                                            //_getch�OŪ���ϥΪ̿�J1�ӴN�����@�ӡ]���O�ϥΪ̨S��J�ɷ|�@���d�b�o�@��{���W�^
		if(_getch() != 'q') {
			//���o��e�ɶ��]�U2��^
			time(&time1);
			tm1 = localtime(&time1); 
			printf("%2d:%2d:%2d\n", tm1->tm_hour, tm1->tm_min, tm1->tm_sec);
		}
	}

	system("pause");
	return 0;
}