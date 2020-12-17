import requests
import json


def get_balance(companyName):
	return requests.get("https://www.justroll.net/Balance/" + companyName)

def get_company(companyName=""):
	return requests.get("https://www.justroll.net/Company/" + companyName)

def get_booking():
	return requests.get("https://www.justroll.net/Booking")

def post_booking(companyName, message, accountFromNumber, accountToNumber, amountCHF):
	body = {
				"companyName" : companyName,
				"message" : message,
				"accountFromNumber" : accountFromNumber,
				"accountToNumber" : accountToNumber,
				"amountCHF" : amountCHF
			}

	return requests.post(url="https://www.justroll.net/Booking", json=body)

def post_company(companyName):
	body = {
				"name": companyName,
  				"additionalAccounts": []
			}

	return requests.post(url="https://www.justroll.net/Company", json=body)



def main():

	print(get_booking().json())

if __name__ == "__main__": main()
