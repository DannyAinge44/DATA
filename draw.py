# interface generator for data.py

# imports
import os

# draws banner and list of commands
def header(commands):
	stars = "*****************************************************"
	title = "DATA – Distributed Accounting through Terminal Access"
	print(stars, title, stars, sep="\n", end="\n\n\n")

	print("Commands [Parameters]")
	for c in commands:
		print(f"– {c}")
	print("")

# clears interface between loops
# conditional to ensure function works on all OS
# source:  https://stackoverflow.com/questions/517970/how-to-clear-the-interpreter-console
def cls():
    os.system('cls' if os.name=='nt' else 'clear')

# takes array as input, prints each element with a blank space
def output(lst):
	for e in lst:
		print(e, end="\n")

# draws total interface
def interface(commands, content):
	cls()
	header(commands)
	output(content)
