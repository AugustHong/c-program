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

	char suit[4][5] = { { "�®�" },{ "�R��" },{ "�٧�" },{ "����" } };

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


//���card�i�令�}�C�Ӧs�A�Ycard[51]�M���struct �̪��אּsuit[5]�Mnumber�Y�i��1�Ӱ}�C�s�����]ex:card[3].number�K�K���^