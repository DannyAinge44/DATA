# DATA

## Imports
import sys
from datetime import datetime
now = datetime.now()

import draw
import api_request as api_r


## Parsing user input, calling appropriate funktions
# implemented using if elif commands, as python does not support native case : switch
def parse(user_input):
	# user input is split into command and args
	command = user_input.split(" ")[0].lower()
	args = user_input.split(" ")[1:]

	if command == "ls-companies": show_companies(args)
	elif command == "mk-company": make_company(args)
	elif command == "balance": show_balance(args)
	elif command == "pl-statement": show_pls(args)
	elif command == "history": show_history(args)
	elif command == "book": book(args)

	elif command == "info": info()
	elif command == "help": help_me()
	elif command == "clear" or command == "cls": draw.cls()
	elif command == "quit": exit_seq()

	else:
		error("could not parse command")


def show_companies(args):
	if len(args) != 0:
		error("incorrect number of parameters passed, 0 expected")
	else:
		response = api_r.get_company()
		prt_out_content.append(f"Response Code: {response.status_code}\n")

		# catch invalid return (should never happen)
		try:
			rsp = response.json()
		except IndexError:
			error("unexpected error")
		else:
			prt_out_content.append("LIST OF")
			prt_out_content.append("EXISTING COMPANIES")
			prt_out_content.append("==================")
			for i in rsp:
				prt_out_content.append(i["companyName"].upper())
			prt_out_content.append("==================")

def make_company(args):
	if len(args) != 1:
		error("incorrect number of parameters passed, 1 expected")
	else:
		response = api_r.post_company(args[0])
		prt_out_content.append(f"Response Code: {response.status_code}\n")
		if response.status_code == 200:
			prt_out_content.append("Success\n")
		else:
			prt_out_content.append(response.text)



def show_balance(args):
	# catch invalid number of parameters
	if len(args) != 1:
		error("incorrect number of parameters passed, 1 expected")
	else:
		response = api_r.get_balance(args[0])
		prt_out_content.append(f"Response Code: {response.status_code}\n")

		# catch invalid input (company name)
		try:
			rsp = response.json()[0]
		except IndexError:
			error("company not found")
		# compose output
		else:
			comp_name = rsp["companyName"].upper()
			prt_out_content.append(f"BALANCE SHEET – {comp_name}")
			prt_out_content.append("=============================================")
			sub_header = "{:<4} {:<30} {:>9}".format("#", "Account", "Balance")
			prt_out_content.append(sub_header)
			prt_out_content.append("---------------------------------------------")

			for acc in rsp["accounts"]:
				if acc["accountNumber"] <= 1000:
					num = str(acc["accountNumber"])
					acc_name = str(acc["name"])
					value = str(acc["balance"])

					# ensuring correct alignment
					line = "{:<4} {:<30} {:>9}".format(num, acc_name, value)
					prt_out_content.append(line)
			prt_out_content.append("=============================================\n")


def show_history(args):
	# catch invalid number of parameters
	if len(args) > 1:
		error("too many parameters passed, 1 expected")

	if len(args) == 1:
		try:
			limit = int(args[0]) * -1
		except:
			error("invalid limit, must be valid int")
			return 0
	else:
		limit = -0

	response = api_r.get_booking()
	prt_out_content.append(f"Response Code: {response.status_code}\n")

	# catch invalid response from server
	try:
		rsp = response.json()[limit:]
	except IndexError:
		error("unexpected response, please try again")
	# compose output
	else:
		prt_out_content.append(f"HISTORY")
		prt_out_content.append("================================================================================")
		line = "{:<8} {:<4} {:>22} / {:<22} {:>4} {:>12} | {:<2}".format("COMPANY", "#", "FROM", "TO", "#", "CHF", "DATE")
		prt_out_content.append(line)
		prt_out_content.append("--------------------------------------------------------------------------------")
		for bookings in rsp:
			name = bookings["companyName"].upper()
			nr_from = bookings["accountFromNumber"]
			acc_from = bookings["accountFromName"]
			nr_to = bookings["accountToNumber"]
			acc_to = bookings["accountToName"]
			amount = bookings["amountCHF"]
			text = bookings["message"]

			line = "{:<8} {:<4} {:>22} / {:<22} {:>4} {:>12} | {:<2}".format(name, nr_from, acc_from, acc_to, nr_to, amount, text)

			prt_out_content.append(line)
		prt_out_content.append("================================================================================")


def book(args):
	# catch invalid number of parameters
	if len(args) < 4:
		error("incorrect number of parameters passed, 4 expected")

	else:
		# casting parameters to appropirate types, catching and displaying errors
		try:
			name = str(args[0])
		except:
			error("Company Name must be a valid string")
			return 0
		try:
			nr_from = int(args[1])
		except:
			error("#From must be a valid integer")
			return 0
		try:
			nr_to = int(args[2])
		except:
			error("#To must be a valid integer")
			return 0
		try:
			amount = float(args[3])
		except:
			error("Amount must be a valid float")
			return 0


		# assuming inputs were correct, execute booking
		response = api_r.post_booking(name, now.strftime("%m/%d/%Y, %H:%M:%S"),
		                              nr_from, nr_to, amount)
		prt_out_content.append(f"Response Code: {response.status_code}")
		if response.status_code == 200:
			prt_out_content.append("Success\n")
		else:
			prt_out_content.append(response.text)


def show_pls(args):
	# catch invalid number of parameters
	if len(args) != 1:
		error("incorrect number of parameters, 1 expected")

	else:
		response = api_r.get_company(str(args[0]))
		prt_out_content.append(f"Response Code: {response.status_code}\n")

		# catch invalid input (company name)
		try:
			rsp = response.json()[0]
		except IndexError:
			error("company not found")
		else:
		# compose output
			comp_name = rsp["companyName"].upper()
			prt_out_content.append(f"PROFIT/LOSS STATEMENT – {comp_name}")
			prt_out_content.append("========================================")
			sub_header = "{:<4} {:<25} {:>9}".format("#", "Account", "Balance")
			prt_out_content.append(sub_header)
			prt_out_content.append("----------------------------------------")

			for acc in rsp["accounts"]:
				if acc["accountNumber"] >= 1000:
					num = str(acc["accountNumber"])
					acc_name = str(acc["name"])
					value = str(acc["balance"])

					# ensuring correct alignment
					line = "{:<4} {:<25} {:>9}".format(num, acc_name, value)
					prt_out_content.append(line)
			prt_out_content.append("========================================\n")


def info():
	with open("README.txt", "r", encoding="utf-8") as f:
		prt_out_content.append(f.read())

def help_me():
	with open("HELP.txt", "r", encoding="utf-8") as f:
		prt_out_content.append(f.read())

def exit_seq():
	draw.cls()
	print("Thank you for using DATA - Goodbye")
	sys.exit(0)

def error(error_string):
	prt_out_content.append("") # empty line
	prt_out_content.append(f"Parsing Error – {error_string}\n")
	prt_out_content.append("Please re-enter your command")
	prt_out_content.append("Paramaters should be deliniated by spaces")



# setup of global vars
# list of functioning commands
commands = ["ls-companies",
			"mk-company [Company Name]",
			"balance [Company Name]",
			"pl-statement [Company Name]",
			"history [Limit]",
			"book [Company Name, From, To, Amount]",
			"info", "help", "clear", "quit"]
# default printout is empty
prt_out_content = []

while True:
	draw.interface(commands, prt_out_content)

	# strip user input to avoid errors due to space at end of line
	user_input = input(">> ").strip()

	# start the assembly of the output of given command
	prt_out_content = []
	prt_out_content.append(f"Previous Command: {user_input}")

	parse(user_input)
