MEMORY
{
	flash : org = 0x00000000, len = 256k
	ram : org = 0x20000000, len = 64k
}

SECTIONS
{
	.text : { *(.text*) } > flash
	.bss : { *(.bss*) } > ram
}
