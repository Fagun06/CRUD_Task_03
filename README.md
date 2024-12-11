# Task 3 - Order Management API

## Description
This project provides an API for managing orders and their associated items. It supports CRUD operations, filtering, pagination, and bulk insertion. Additionally, the API enables calculating total sales grouped by date.

---

## Features

### Order Management
- **Add a New Order**: Create an order along with its items.
- **Update an Order**: Modify an order and its associated items, including:
  - Updating the quantity of existing items.
  - Adding new items to the order.
  - Removing items from the order.
- **Delete an Order**: Perform a cascade delete to remove the order and all associated items.
- **Retrieve Order Details**: Fetch details of an order, including its items.

### Filtering and Pagination
- **Filters for Orders**:
  - By date range (`OrderDate` between two dates).
  - By total amount (`TotalAmount` between minimum and maximum values).
  - By customer name (`CustomerName`).
- **Pagination**:
  - Use `pageNumber` and `pageSize` for paginated order retrieval.

### Sales Summary
- Retrieve total sales (sum of `TotalAmount`) within a date range.
- Group sales results by `OrderDate` for daily totals.

### Bulk Operations
- **Bulk Insert**: Add multiple orders and their associated items in a single API call.

---

## Database Schema

### OrderHeader
| Column         | Type       | Constraints         |
|----------------|------------|---------------------|
| `OrderId`      | `int`      | Primary Key         |
| `CustomerName` | `nvarchar` | Required            |
| `OrderDate`    | `datetime` |                     |
| `TotalAmount`  | `decimal`  |                     |

### OrderRows
| Column         | Type       | Constraints                             |
|----------------|------------|-----------------------------------------|
| `OrderItemId`  | `int`      | Primary Key                             |
| `OrderId`      | `int`      | Foreign Key referencing `OrderHeader`   |
| `ProductName`  | `nvarchar` | Required                                |
| `Quantity`     | `int`      | Required                                |
| `UnitPrice`    | `decimal`  | Required                                |

---

## API Endpoints


## API Endpoints

### Orders
- **POST /api/Order/CreateOrderWithItem**: Create a new order with items.
- **POST /api/Order/UpdateOrderWithItem**: Update an existing order and its items.
- **GET /api/Order/GetOrderDatails**: Retrieve details of a specific order.
- **PUT /api/Order/DeleteOrder**: Delete an order and its associated items.
- **GET /api/Order/MinimumAndMaximum**: Retrieve orders based on minimum and maximum total amount.
- **GET /api/Order/MinimumMaximumDetails**: Retrieve details for orders filtered by minimum and maximum values.
- **GET /api/Order/SearchByCustormerName**: Search orders by customer name.
- **GET /api/Order/GetByDateTime**: Retrieve orders within a specific date range.
- **GET /api/Order/GetByDateTimeTotalAmount**: Get total amounts for orders within a date range.
- **GET /api/Order/GetDailyTotalSales**: Retrieve daily total sales grouped by order date.
- **POST /api/Order/CreateOrdersWithItemBulkInsert**: Perform a bulk insert of orders and items.
- **GET /api/Order/OrderListByPagination**: Retrieve a paginated list of orders.
- **GET /api/Order/FiltersByDateTimeMinMaxCustormerName**: Filter orders by date range, total amount range, and customer name.
- **GET /api/Order/GetByDateTimeTotalAmountRang**: Retrieve orders filtered by a specific total amount range within a date range.

### Sales
- **GET /sales/summary**: Get total sales grouped by date within a date range.

### Bulk Operations
- **POST /orders/bulk**: Perform a bulk insert of orders and items.

---

## How to Run
### Steps
1. Clone the repository:
   ```bash
   git clone <[repository_url](https://github.com/Fagun06/Order-Management-System-API)>
  
   ```
2. Configure the database connection in the `config` file.
3. Run the application:
   ```bash
   npm start
   # or for Python
   python app.py
   ```
---

## Examples

### Create Order
#### Request
```json
POST /orders
{
  "CustomerName": "John Doe",
  "OrderDate": "2024-12-11",
  "TotalAmount": 150.00,
  "Items": [
    { "ProductName": "Product A", "Quantity": 2, "UnitPrice": 25.00 },
    { "ProductName": "Product B", "Quantity": 1, "UnitPrice": 100.00 }
  ]
}
```

#### Response
```json
{
  "OrderId": 1,
  "Message": "Order created successfully."
}
```

---

### Retrieve Orders with Filters
#### Request
```json
GET /orders?pageNumber=1&pageSize=5&CustomerName=John&minTotalAmount=100&maxTotalAmount=500
```

#### Response
```json
{
  "Page": 1,
  "PageSize": 5,
  "TotalCount": 10,
  "Orders": [
    {
      "OrderId": 1,
      "CustomerName": "John Doe",
      "OrderDate": "2024-12-11",
      "TotalAmount": 150.00,
      "Items": [
        { "ProductName": "Product A", "Quantity": 2, "UnitPrice": 25.00 },
        { "ProductName": "Product B", "Quantity": 1, "UnitPrice": 100.00 }
      ]
    }
  ]
}
```

---

