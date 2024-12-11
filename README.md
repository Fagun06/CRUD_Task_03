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

### Orders
- **POST /orders**: Create a new order with items.
- **PUT /orders/{orderId}**: Update an existing order and its items.
- **DELETE /orders/{orderId}**: Delete an order and its associated items.
- **GET /orders/{orderId}**: Retrieve details of a specific order.
- **GET /orders**: Retrieve a list of orders with filtering and pagination.

### Sales
- **GET /sales/summary**: Get total sales grouped by date within a date range.

### Bulk Operations
- **POST /orders/bulk**: Perform a bulk insert of orders and items.

---

## How to Run
### Steps
1. Clone the repository:
   ```bash
   git clone <repository_url>
   cd <repository_folder>
   ```
2. Install dependencies:
   ```bash
   npm install
   # or for Python
   pip install -r requirements.txt
   ```
3. Configure the database connection in the `config` file.
4. Run the application:
   ```bash
   npm start
   # or for Python
   python app.py
   ```
5. Access the API at `http://localhost:3000` (default).

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

