Feature: DeleteProduct

Como: Empleado del sistema
Quiero: dar de baja un producto existente
Para: que el producto ya no este en el sistema

@deleteProduct
Scenario: Successful product deletion
    Given I am an authorized employee deleting a product
    When I choose the product with code "<codeProduct>" to be deleted
    Then the response status code should be "<codeResponse>"

Examples: 
    | codeProduct | codeResponse |
    | 12370       | 200          |
    | 12371       | 200          |