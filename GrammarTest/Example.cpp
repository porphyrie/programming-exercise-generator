#define _CRT_SECURITY_NO_WARNINGS

#include <iostream>
#include <stdlib.h>
#include <time.h>
#include <conio.h>

using namespace std;

struct nod {
	int n;
	nod* prec;
};

nod* vf;

void adauga(int m) {
	if (vf == NULL) {
		vf = new nod;
		vf->n = m;
		vf->prec = NULL;
	}
	else {
		nod* p = new nod;
		p->n = m;
		p->prec = vf;
		vf = p;
	}
}

void sterge() {
	nod* q = vf;
	vf = vf->prec;
	delete q;
}

void listeaza() {
	nod* p = vf;
	while (p) {
		cout << p->n << ' ';
		p = p->prec;
	}
}

nod* cauta(int m) {
	nod* q = vf;
	while (q != NULL) {
		if (q->n == m)
			return q;
		q = q->prec;
	}
	return NULL;
}

void genereaza_output() {
    nod *p = vf;
    int i = 0;
    while (p) {
        i *= p->n;
        if (p->n % 3 == 0)
            p->n += 3;
        p = p->prec;
    }
}
int main() {

	srand(time(NULL));

	int nr_elem = rand() % 3 + 4;

	for (int i = 0; i < nr_elem; i++) {
		int m = rand() % 10;
		if (!cauta(m))
			adauga(m);
		else
			i--;
	}

	printf("\n#INAINTE#\n");
	listeaza();
	printf("\n#INAINTE#\n");
	
	genereaza_output();

	printf("\n#DUPA#\n");
	listeaza();
	printf("\n#DUPA#\n");

	//_getch();

	return 1;
}
