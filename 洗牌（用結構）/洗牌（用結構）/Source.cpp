#include<stdio.h>
#include<stdlib.h>
#include<string.h>
#include<math.h>
#include<time.h>
#include<ctype.h>

struct _card
{
	char suit[52][5];
	int number[52];
};


int main() {
	struct _card card;
	int a, b;
	char r[9];
	srand(time(NULL));

	char suit[4][5] = { { "黑桃" },{ "愛心" },{ "菱形" },{ "梅花" } };

	for (int i = 0; i <= 3; i++) {
		for (int j = 1; j <= 13; j++) {
			strcpy(card.suit[i * 13 - 1 + j], suit[i]);
			card.number[i * 13 - 1 + j] = j;
			//printf("%s %d\n", card.suit[i * 13 - 1 + j], card.number[i * 13 - 1 + j]);
		}
	}

	for (int k = 51; k >= 0; k--) {
		a = rand() % (k + 1);
		printf("%s %d\n", card.suit[a], card.number[a]);


		b = card.number[a];
		card.number[a] = card.number[k];
		card.number[k] = b;

		strcpy(r, card.suit[a]);
		strcpy(card.suit[a], card.suit[k]);
		strcpy(card.suit[k], r);

	}


	system("pause");
	return 0;
}


//其實card可改成陣列來存，即card[51]然後把struct 裡的改為suit[5]和number即可用1個陣列存全部（ex:card[3].number……等）