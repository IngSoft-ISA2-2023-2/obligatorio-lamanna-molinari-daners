Feature: PurchaseProduct

Como: cliente
Quiero: realizar una compra que incluye productos
Para: que la compra quede registrada en el sistema


@successfulPurchase
Scenario: Successful purchase
	Given I'm a client
	When I enter a purhcase request
	Then the response code should be  "200"