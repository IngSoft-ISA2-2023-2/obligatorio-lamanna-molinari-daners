Feature: UpdateProduct

Como: Empleado del sistema
Quiero: Crear un nuevo producto
Para: Poder verlo en el sistema

@productCreation

Scenario: Successful product update
    Given I am an authorized employee who selected the product with code "<oldCode>"
    When I update a product with the new name "<name>" 
    And the new description "<description>"
    And the new code "<code>"
    And the new price "<price>"
    Then the response code should be "<codeResponse>"

Examples: 
    | name              | description              | code  | price | controller | codeResponse | token                                | codeMessage | oldCode |
    | new nombre test   | new descripcion prueba   | 12444 | 100   | product    | 200          | E9E0E1E9-3812-4EB5-949E-AE92AC931401 | a           | 12442   |
    | new nombre test 2 | new descripcion prueba 2 | 12445 | 100   | product    | 200          | E9E0E1E9-3812-4EB5-949E-AE92AC931401 | a           | 12443   |
  
Scenario: UnSuccessful product update
    Given I am an authorized employee who selected the product with code "<oldCode>"
    When I update a product with the new name "<name>" 
    And the new description "<description>"
    And the new code "<code>"
    And the new price "<price>"
    Then the error response message should be "<codeMessage>"
     
Examples: 
    | name                            | description                                                                    | code   | price | controller | codeResponse | token                                | codeMessage                                  | oldCode |
    |                                 | descripcion prueba                                                             | 12600  | 100   | product    | 455          | E9E0E1E9-3812-4EB5-949E-AE92AC931401 | The Product was not correctly created.       | 12400   |
    | nombre test                     |                                                                                | 12601  | 100   | product    | 455          | E9E0E1E9-3812-4EB5-949E-AE92AC931401 | The Product was not correctly created.       | 12400   |
    | nombre test                     | descripcion                                                                    |        | 100   | product    | 455          | E9E0E1E9-3812-4EB5-949E-AE92AC931401 | The Product was not correctly created.       | 12400   |
    | nombre muy largo con mas de 30. | descripcion                                                                    | 12602  | 100   | product    | 455          | E9E0E1E9-3812-4EB5-949E-AE92AC931401 | The Product format is incorrect              | 12400   |
    | nombre test                     | Lorem ipsum dolor sit amet, consectetur adipiscing elit. Fusce vitae sagittis. | 12603  | 100   | product    | 455          | E9E0E1E9-3812-4EB5-949E-AE92AC931401 | The Product format is incorrect              | 12400   |
    | nombre test                     | descripcion prueba                                                             | 12604  | -1    | product    | 455          | E9E0E1E9-3812-4EB5-949E-AE92AC931401 | The Product was not correctly created.       | 12400   |
    | nombre                          | descripcion prueba                                                             | 1260   | 100   | product    | 455          | E9E0E1E9-3812-4EB5-949E-AE92AC931401 | The Product format is incorrect              | 12400   |
    | nombre                          | descripcion prueba                                                             | 126000 | 100   | product    | 455          | E9E0E1E9-3812-4EB5-949E-AE92AC931401 | The Product format is incorrect              | 12400   |
    | nombre                          | descripcion prueba                                                             | 12605  | 100   | product    | 455          | E9E0E1E9-3812-4EB5-949E-AE92AC931401 | A product with that code already exists in that pharmacy. | 12400   |
  