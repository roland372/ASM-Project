.code
; void SubtractAddFactorImage(
;    byte* bmpOriginal, = RCX
;    byte* bmpBlur, = RDX
;    int sizeInBytes    = R8


SubtractAddFactorImage proc
    mov r10, 0            ;Set the offset pointer to 0
    mov r11, r8            ;Zapisujemy rozmiar obrazu do zmiennej????nwm czy dziala
    mov r12, 0
    mov r13, 0ffffh
    
SubtractLoop:
    mov al, byte ptr [rcx+r10]     ; Read the next byte from original
    mov bl, byte ptr [rdx+r10]     ; Read the next byte from blurred
    sub al, bl               ; Oryginalny - zblurowany
    cmovc rax, r12               ; Set to 0 on underflow !17
    mov byte ptr [rdx + r10], al     ; Ustaw do zblurowanego

    inc r10                ;Move to the next byte in the arrays
    dec r11                ;Decrement our counter
    jnz SubtractLoop        ;Jump if there's more
    
    mov r10, 0            ;Set the offset pointer to 0
    mov r11, r8            ;Zapisujemy rozmiar obrazu do zmiennej????nwm czy dziala
AddLoop:
    mov al, byte ptr[rcx+r10]    ;Read the next byte from original
    mov bl, byte ptr [rdx+r10]     ; Read the next byte from blurred
    add al, bl             ; Oryginalny + (Oryginalny - zblurowany)
    cmovc rax, r13            ; Set to 255 on overflow !30
    mov byte ptr [rdx+r10], al    ; Dodaj do ostatecznego obrazu
    inc r10                ;Move to the next byte in the arrays
    dec r11                ;Decrement our counter
    jnz AddLoop            ;Jump if there's more

    ret
SubtractAddFactorImage endp
END