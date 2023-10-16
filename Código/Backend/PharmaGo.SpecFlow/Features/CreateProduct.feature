Feature: CreateProduct

Como: Empleado del sistema
Quiero: Crear un nuevo producto
Para: Poder verlo en el sistema

Scenario: : Crear producto válido con todos los datos
    Given The name "Nombre"
    And the description "Descripcion"
    And the code "12345"
    And the price "100"
    When A "product" is created with the values
    Then i receive the response with the "201" and "Producto creado correctamente"
    