# 2048 ve WPF
## Herní okno
Herní okno zobrazuje klikatelné klávesy, pole hry s barevnými políčky a skóre. Hra by měla být replikou již její známe verze, která je dostupná na `https://2048game.com/`. 
## Kód hry
Pracuje se s 2D polem, ve kterém se ukládají čísla, která se spojují. Ze začátku každé hry se nastaví 2 začáteční políčka metodou ```SetupTiles();```.

### VisualizeLabels();
Metoda slouží k zobrazování jednotlivých políček - Labelů v herním poli. Řeší jejich barvu, text, popřípadě i velikost textu, kdyby byl její obsah dlouhý.

### GetTileNumber();
Zajišťuje šanci 1 : 7, že se zrovna objeví políčko s hodnotou 4, namísto s hodnotou 2

### CheckEnd();
Kontroluje konec, je volána po každém pohybu, porovnává, jestli se ještě dá někam hnout.

### ArraysAreEqual();
Prochází a porovnává, jestli jsou 2D pole stejná

### End();
Zavolá restartovací okno

### RestartGame();
Vše přenastaví do hodnot nové hry

### Move____();
Metoda, která pohybuje s polem vpravo, vlevo... podle pravidel hry. Zároveň i upravuje hodnotu skóre.

### Click(); a KeyDown(); metody
Klikatelnost a funkčnost tlačítek (Pokud jde, hne se, vytvoří se nové políčko a zobrazí se).

### TestMove();
Zkouší, jestli se dá hnout - dělá to, co by dělala metoda Move(), jen nenastaví přímo proměnnou Tiles, takže slouží pro porovnávání mezi poli.


## Game Over okno
Zobrazuje skóre a 2 tlačítka, jedno pro restartování hry, jedno pro vypnutí aplikace.

## Kód Restartovacího okna
Metody v něm o sobě názvem vypovídají...