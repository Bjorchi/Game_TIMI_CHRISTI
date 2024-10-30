# Game_TIMI_CHRISTI
 die Buben am game entwickeln

 section .data
    n dd 10                  ; Anzahl der zu berechnenden Fibonacci-Zahlen (hier 10)
    fibs times 20 dd 0       ; Speicherbereich für 20 Zahlen, gefüllt mit Null (Platz für die Fibonacci-Zahlen)
                             ; Wir nehmen an, dass die erste und zweite Fibonacci-Zahl 0 und 1 sind.

section .text
    global _start            ; Einstiegspunkt für den Linker

_start:
    ; Initialisiere die ersten beiden Fibonacci-Zahlen
    mov dword [fibs], 0      ; fibs[0] = 0
    mov dword [fibs + 4], 1  ; fibs[1] = 1

    ; Setze die Schleifenzähler (i = 2)
    mov ecx, 2               ; Startindex für die Berechnung (2. Zahl)

compute_fibonacci:
    ; Berechne die nächste Fibonacci-Zahl: fibs[i] = fibs[i-1] + fibs[i-2]
    mov eax, [fibs + ecx*4 - 4] ; Lade fibs[i-1] in eax
    add eax, [fibs + ecx*4 - 8] ; Addiere fibs[i-2] zu eax

    ; Speichere das Ergebnis in fibs[i]
    mov [fibs + ecx*4], eax

    ; Inkrementiere den Zähler
    inc ecx

    ; Überprüfe, ob wir alle n Fibonacci-Zahlen berechnet haben
    cmp ecx, [n]
    jl compute_fibonacci      ; Springe zurück, wenn ecx < n

exit_program:
    ; Exit-Systemaufruf
    mov eax, 1                ; syscall number für sys_exit (1)
    xor ebx, ebx              ; Rückgabewert 0
    int 0x80     

//XD
