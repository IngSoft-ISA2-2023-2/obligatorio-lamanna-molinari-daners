Feature: CreateProduct

Como: Empleado del sistema
Quiero: Crear un nuevo producto
Para: Poder verlo en el sistema

@productCreation

Scenario: Successful product creation
    Given I am an authorized employee
    When I add a new product with the name "<name>" 
    And the description "<description>"
    And the code "<code>"
    And the price "<price>"
    Then the response status should be "<codeResponse>"

Examples: 
    | name                            | description                                                                    | code   | price | controller | codeResponse | token                                |                           
    | nombre test                     | descripcion prueba                                                             | 12345  | 100   | product    | 200          | E9E0E1E9-3812-4EB5-949E-AE92AC931401 |                                       
    | nombre test 2                   | descripcion prueba 2                                                           | 12346  | 100   | product    | 200          | E9E0E1E9-3812-4EB5-949E-AE92AC931401 |                                      

Scenario: UnSuccessful product creation
    Given I am an authorized employee
    When I add a new product with the name "<name>" 
    And the description "<description>"
    And the code "<code>"
    And the price "<price>" 
    Then the response message should be "<codeMessage>"

Examples: 
    | name                            | description                                                                    | code   | price | controller | codeResponse | token                                | codeMessage                            |
    |                                 | descripcion prueba                                                             | 12347  | 100   | product    | 455          | E9E0E1E9-3812-4EB5-949E-AE92AC931401 | The Product was not correctly created. |
    | nombre test                     |                                                                                | 12348  | 100   | product    | 455          | E9E0E1E9-3812-4EB5-949E-AE92AC931401 | The Product was not correctly created. |
    | nombre test                     | descripcion                                                                    |        | 100   | product    | 455          | E9E0E1E9-3812-4EB5-949E-AE92AC931401 | The Product was not correctly created. |
    | nombre muy largo con mas de 30. | descripcion                                                                    | 12350  | 100   | product    | 455          | E9E0E1E9-3812-4EB5-949E-AE92AC931401 | The Product format is incorrect        |
    | nombre test                     | Lorem ipsum dolor sit amet, consectetur adipiscing elit. Fusce vitae sagittis. | 12351  | 100   | product    | 455          | E9E0E1E9-3812-4EB5-949E-AE92AC931401 | The Product format is incorrect        |
    | nombre test                     | descripcion prueba                                                             | 12352  | -1    | product    | 455          | E9E0E1E9-3812-4EB5-949E-AE92AC931401 | The Product was not correctly created. |
    | nombre                          | descripcion prueba                                                             | 1234   | 100   | product    | 455          | E9E0E1E9-3812-4EB5-949E-AE92AC931401 | The Product format is incorrect        |
    | nombre                          | descripcion prueba                                                             | 123456 | 100   | product    | 455          | E9E0E1E9-3812-4EB5-949E-AE92AC931401 | The Product format is incorrect        |
    | nombre                          | descripcion prueba                                                             | 12353 | 100   | product     | 455          | E9E0E1E9-3812-4EB5-949E-AE92AC931401 | The product already exists in that pharmacy.     |
  