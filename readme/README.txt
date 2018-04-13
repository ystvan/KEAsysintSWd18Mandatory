IOT sensor: 

	measuring kitchen light, temperature
	UDP broadcast service
	Hotel Security sending measurements from:
	"http://localhost:5000"

Proxy:

	City 911 Emergency Service
	Listening on network for incoming sensor broadcasts
	ports: 5000 (plain text),
		   5001 (JSON),
		   5002 (XML)

SOAP service on Azure Cloud SQL server:

	Services offered:
		saving data to cloud (SQL server v.12 "sysint.database.windows.net")
		getting data from cloud (SQL server v.12 "sysint.database.windows.net")
		WSDL endpoint: http://sysint-mand.azurewebsites.net/Service1.svc?singleWsdl

Web Application:

	users consume from their favourite browsers
	user logs in with KEA credentials
	Can see all measurements stored in Azure DB

Identity Server

	redirects to KEA Azure Active Directory, 
	user authenticats with OAuth2 & OpenID Connect => asks User for consent (getting user info as a token)
	sends the id token back to the Web Application

KEA Azure Active Directory

	handled and managed by KEA IT
	Office 365 accounts,
	Microsoft accounts (all KEA affiliates such as faculty staff, teachers, students...)

REST API:

	CRUD measurements from Azure DB
	See server logs...

	REST 6 constraints: 

		1. client server constraint (client abd server seperated)
		2. statelessness (state is contained within the request)
		3. cachable (each rsponse message must explicitly state is it cachable or not)
		4. layered system (client cannot tell what layer it's connected to)
		5. code on demand (server can extend client functionality)
		6. uniform interface (api and consumers share one single technical interface: URI, Method, Media Type)
			6.1 identification of resources (resources are conceptually seperate from representation (json is != server representation)
			6.2 manipulation of resources through representations (repr + metadata should be sufficient to modify or delete resource)
			6.3 self-descriptive msg (each msg must include enough info to describe how to process it) 
			6.4 HATEOAS (how to consume and use the api)


Twitter API:

	Connect to twitter account, tweet measurements to user's feed

Twitter account:

	See measurements, live update, can be followed to get the feed