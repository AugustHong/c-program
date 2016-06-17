#include<stdio.h>
#include<stdlib.h>
#include<time.h>

int main(){
	int card[52], a=0, x = 0;
	srand(time(NULL));

	for (int i = 0; i <= 51; i++){ card[i] = i; }

	for (int j = 51; j >= 0; j--){
		a = rand() % (j+1);
		if (card[a] >= 0 && card[a] <= 12){ printf("黑桃%d\n", (card[a] % 13) + 1); }
		if (card[a] >= 13 && card[a] <= 25){ printf("愛心%d\n", (card[a] % 13) + 1); }
		if (card[a] >= 26 && card[a] <= 38){ printf("方塊%d\n", (card[a] % 13) + 1); }
		if (card[a] >= 39 && card[a] <= 51){ printf("梅花%d\n", (card[a] % 13) + 1); }

		x = card[a];   //change
		card[a] = card[j];
		card[j] = x;
	}

	system("pause");
	return 0;

}