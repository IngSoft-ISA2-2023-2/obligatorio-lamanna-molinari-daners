Feature: CreateProduct

Como: Empleado del sistema
Quiero: Crear un nuevo producto
Para: Poder verlo en el sistema

Scenario: : Crear producto válido con todos los datos
    Given The name "<nombre>"
    And la descripcion "<descripcion>"
    And el codigo "<codigo>"
    And el precio "<precio>"
    When Un product es creado por el empleado con el token "<token>" mediante el "<controller>" con el endpoint
    Then recibo una respuesta con el codigo "<codeResponse>"

Examples: 
    | nombre                          | descripcion                                                                    | codigo | precio | controller | codeResponse | token                                |
    | nombre test                     | descripcion prueba                                                             | 12345  | 100    | product    | 200          | E9E0E1E9-3812-4EB5-949E-AE92AC931401 |
    | nombre test 2                   | descripcion prueba 2                                                           | 12346  | 100    | product    | 200          | E9E0E1E9-3812-4EB5-949E-AE92AC931401 |
    |                                 | descripcion prueba                                                             | 12347  | 100    | product    | 400          | E9E0E1E9-3812-4EB5-949E-AE92AC931401 |
    | nombre test                     |                                                                                | 12348  | 100    | product    | 400          | E9E0E1E9-3812-4EB5-949E-AE92AC931401 |
    | nombre test                     | descripcion                                                                    |        | 100    | product    | 400          | E9E0E1E9-3812-4EB5-949E-AE92AC931401 |
    | nombre muy largo con mas de 30. | descripcion                                                                    | 12350  | 100    | product    | 400          | E9E0E1E9-3812-4EB5-949E-AE92AC931401 |
    | nombre test                     | Lorem ipsum dolor sit amet, consectetur adipiscing elit. Fusce vitae sagittis. | 12351  | 100    | product    | 400          | E9E0E1E9-3812-4EB5-949E-AE92AC931401 |
    | nombre test                     | descripcion prueba                                                             | 12352  | -1     | product    | 400          | E9E0E1E9-3812-4EB5-949E-AE92AC931401 |
    | nombre                          | descripcion prueba                                                             | 1234   | 100    | product    | 400          | E9E0E1E9-3812-4EB5-949E-AE92AC931401 |
    | nombre                          | descripcion prueba                                                             | 123456 | 100    | product    | 400          | E9E0E1E9-3812-4EB5-949E-AE92AC931401 |    
    