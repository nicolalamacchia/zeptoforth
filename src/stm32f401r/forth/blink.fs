\ Turnkey LED blink for the STM32F401R "Black Pill" (LED on PC13).
\
\ Uses the `turnkey` hook: the kernel looks up a word named "turnkey" and runs
\ it AFTER the whole `init` chain has completed (system fully up), right before
\ the REPL. That is the correct place for application startup -- unlike `init`,
\ which runs too early and must not be used to start tasks.
\
\ This `turnkey` spawns the blink task and RETURNS, so the REPL still comes up
\ and the board stays usable over swdcom. Because it returns, it can never hang
\ the boot the way an init-based version can.
\
\ Load on top of a mini_swdcom (or full) build. To undo: restore a snapshot,
\ or erase-all / reflash.

compile-to-flash
gpio import
task import

\ Flip PC13 (drive low if currently high, high if currently low)
: led-toggle ( -- ) 13 GPIOC ODR@ if 13 GPIOC BR! else 13 GPIOC BS! then ;

\ The blink loop, run as its own task
: blink-task ( -- ) begin led-toggle 250 ms again ;

\ Runs at boot AFTER init, then returns -> REPL stays available
: turnkey ( -- )
  OUTPUT_MODE 13 GPIOC MODER!
  0 ['] blink-task 320 256 512 spawn run   \ [']  (compile-time tick), NOT '
;

compile-to-ram
