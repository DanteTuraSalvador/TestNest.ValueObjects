# Azure DevOps Work Items - TestNest.ValueObjects

This document contains a comprehensive backlog for the TestNest.ValueObjects project, organized into Epics, Features, User Stories, and Tasks. Use this to manually create work items in Azure DevOps at `https://dev.azure.com/tengtium-io/`.

---

## Epic 1: Value Object Library Foundation

**Description:** Build a robust, reusable Value Object base library for .NET that enables domain-driven design patterns and eliminates primitive obsession.

**Acceptance Criteria:**
- Abstract ValueObject base class with value-based equality
- At least one reference implementation (Currency)
- Comprehensive unit tests with high coverage
- Documentation and examples

---

### Feature 1.1: ValueObject Base Class

**Description:** Create an abstract base class that provides value-based equality semantics for all derived value objects.

**Parent:** Epic 1

#### User Story 1.1.1: Implement Value-Based Equality

**Description:** As a developer, I want a base class that compares objects by their values instead of references, so that two objects with the same values are considered equal.

**Acceptance Criteria:**
- `Equals()` method compares atomic values
- `==` and `!=` operators work correctly
- `GetHashCode()` is consistent with equality
- Null comparisons are handled safely

**Status:** Done

##### Tasks:
| ID | Task | Status |
|----|------|--------|
| 1.1.1.1 | Create ValueObject abstract class | Done |
| 1.1.1.2 | Implement IEquatable<ValueObject> interface | Done |
| 1.1.1.3 | Override Equals(object) method | Done |
| 1.1.1.4 | Implement == and != operators | Done |
| 1.1.1.5 | Implement GetHashCode() using atomic values | Done |
| 1.1.1.6 | Define abstract GetAtomicValues() method | Done |

---

### Feature 1.2: Currency Value Object

**Description:** Implement a Currency value object as the first reference implementation demonstrating the pattern.

**Parent:** Epic 1

#### User Story 1.2.1: Create Currency with Validation

**Description:** As a developer, I want to create Currency objects that validate their inputs, so that invalid currencies cannot exist in the system.

**Acceptance Criteria:**
- Currency code must be exactly 3 uppercase characters
- Only supported currencies are allowed (USD, PHP, EUR, GBP, JPY)
- Symbol cannot be null or empty
- Invalid inputs throw descriptive exceptions

**Status:** Done

##### Tasks:
| ID | Task | Status |
|----|------|--------|
| 1.2.1.1 | Create Currency sealed class extending ValueObject | Done |
| 1.2.1.2 | Add Code and Symbol properties | Done |
| 1.2.1.3 | Implement private constructors | Done |
| 1.2.1.4 | Create static factory method Create(code, symbol) | Done |
| 1.2.1.5 | Add validation for currency code | Done |
| 1.2.1.6 | Add validation for currency symbol | Done |
| 1.2.1.7 | Create CurrencyException with error codes | Done |

#### User Story 1.2.2: Parse Currency from String

**Description:** As a developer, I want to parse currencies from string codes, so that I can easily convert user input or database values to Currency objects.

**Acceptance Criteria:**
- Parse() converts valid codes to Currency instances
- Parse() throws on invalid codes
- TryParse() returns false for invalid codes without throwing
- Case-insensitive parsing

**Status:** Done

##### Tasks:
| ID | Task | Status |
|----|------|--------|
| 1.2.2.1 | Implement Parse(string code) method | Done |
| 1.2.2.2 | Implement TryParse(string, out Currency) method | Done |
| 1.2.2.3 | Add case-insensitive code matching | Done |
| 1.2.2.4 | Return predefined instances for known currencies | Done |

#### User Story 1.2.3: Predefined Currency Instances

**Description:** As a developer, I want predefined static instances for common currencies, so that I can use them without creating new objects.

**Acceptance Criteria:**
- Static readonly instances for USD, PHP, EUR, GBP, JPY
- Empty singleton instance for null-object pattern
- Default currency property

**Status:** Done

##### Tasks:
| ID | Task | Status |
|----|------|--------|
| 1.2.3.1 | Create static readonly Currency instances | Done |
| 1.2.3.2 | Implement lazy Empty singleton | Done |
| 1.2.3.3 | Add Default property | Done |
| 1.2.3.4 | Implement IsEmpty() method | Done |

---

### Feature 1.3: Domain Exception Handling

**Description:** Create a structured exception system for domain validation errors.

**Parent:** Epic 1

#### User Story 1.3.1: Currency Exception with Error Codes

**Description:** As a developer, I want domain exceptions with specific error codes, so that I can handle different error types programmatically.

**Acceptance Criteria:**
- ErrorCode enum with specific error types
- Factory methods for creating exceptions
- Clear error messages

**Status:** Done

##### Tasks:
| ID | Task | Status |
|----|------|--------|
| 1.3.1.1 | Create CurrencyException sealed class | Done |
| 1.3.1.2 | Define ErrorCode enum | Done |
| 1.3.1.3 | Create InvalidCurrencyCode factory method | Done |
| 1.3.1.4 | Create InvalidCurrencySymbol factory method | Done |
| 1.3.1.5 | Create NullCurrency factory method | Done |

---

### Feature 1.4: Unit Testing

**Description:** Comprehensive test coverage for all value object functionality.

**Parent:** Epic 1

#### User Story 1.4.1: Currency Unit Tests

**Description:** As a developer, I want comprehensive unit tests for Currency, so that I can be confident the implementation is correct.

**Acceptance Criteria:**
- Tests for all public methods
- Edge case coverage
- Equality and hash code tests

**Status:** Done

##### Tasks:
| ID | Task | Status |
|----|------|--------|
| 1.4.1.1 | Set up xUnit test project | Done |
| 1.4.1.2 | Write tests for Currency.Create() | Done |
| 1.4.1.3 | Write tests for Currency.TryCreate() | Done |
| 1.4.1.4 | Write tests for Currency.Parse() | Done |
| 1.4.1.5 | Write tests for Currency.TryParse() | Done |
| 1.4.1.6 | Write equality comparison tests | Done |
| 1.4.1.7 | Write hash code consistency tests | Done |
| 1.4.1.8 | Write validation/exception tests | Done |

---

## Epic 2: Extended Value Objects

**Description:** Expand the library with additional commonly-used value objects to demonstrate patterns and provide reusable components.

**Acceptance Criteria:**
- Multiple value object implementations
- Consistent patterns across all implementations
- Full test coverage for each

---

### Feature 2.1: Price Value Object

**Description:** Create a Price value object that combines monetary amount with currency.

**Parent:** Epic 2

#### User Story 2.1.1: Create Price with Validation

**Description:** As a developer, I want a Price value object that enforces business rules for pricing, so that invalid prices cannot exist.

**Acceptance Criteria:**
- Standard and peak price components
- Prices cannot be negative
- Peak price must be >= standard price
- Currency is required

**Status:** New

##### Tasks:
| ID | Task | Status |
|----|------|--------|
| 2.1.1.1 | Create Price sealed class extending ValueObject | New |
| 2.1.1.2 | Add StandardPrice, PeakPrice, Currency properties | New |
| 2.1.1.3 | Implement Create() factory method with validation | New |
| 2.1.1.4 | Create PriceException with error codes | New |
| 2.1.1.5 | Implement Empty/Zero singleton | New |

#### User Story 2.1.2: Price Immutable Updates

**Description:** As a developer, I want fluent methods to create modified Price copies, so that I can work with immutable objects easily.

**Acceptance Criteria:**
- WithStandardPrice() returns new instance
- WithPeakPrice() returns new instance
- WithCurrency() returns new instance
- Original object is unchanged

**Status:** New

##### Tasks:
| ID | Task | Status |
|----|------|--------|
| 2.1.2.1 | Implement WithStandardPrice() method | New |
| 2.1.2.2 | Implement WithPeakPrice() method | New |
| 2.1.2.3 | Implement WithCurrency() method | New |
| 2.1.2.4 | Write unit tests for With* methods | New |

---

### Feature 2.2: Email Value Object

**Description:** Create an Email value object with proper validation.

**Parent:** Epic 2

#### User Story 2.2.1: Create Email with Validation

**Description:** As a developer, I want an Email value object that validates email format, so that invalid emails are rejected.

**Acceptance Criteria:**
- Basic email format validation
- Normalization (lowercase)
- Meaningful error messages

**Status:** New

##### Tasks:
| ID | Task | Status |
|----|------|--------|
| 2.2.1.1 | Create Email sealed class | New |
| 2.2.1.2 | Implement Create() with validation | New |
| 2.2.1.3 | Implement TryCreate() safe version | New |
| 2.2.1.4 | Add email format validation | New |
| 2.2.1.5 | Normalize to lowercase | New |
| 2.2.1.6 | Create EmailException | New |
| 2.2.1.7 | Write unit tests | New |

---

### Feature 2.3: PhoneNumber Value Object

**Description:** Create a PhoneNumber value object with country code support.

**Parent:** Epic 2

#### User Story 2.3.1: Create PhoneNumber with Validation

**Description:** As a developer, I want a PhoneNumber value object that validates and formats phone numbers, so that phone data is consistent.

**Acceptance Criteria:**
- Country code and number components
- Validation for common formats
- Formatted output

**Status:** New

##### Tasks:
| ID | Task | Status |
|----|------|--------|
| 2.3.1.1 | Create PhoneNumber sealed class | New |
| 2.3.1.2 | Add CountryCode and Number properties | New |
| 2.3.1.3 | Implement Create() with validation | New |
| 2.3.1.4 | Implement Parse() from string | New |
| 2.3.1.5 | Create PhoneNumberException | New |
| 2.3.1.6 | Write unit tests | New |

---

### Feature 2.4: Money Value Object

**Description:** Create a Money value object for monetary calculations.

**Parent:** Epic 2

#### User Story 2.4.1: Create Money with Arithmetic

**Description:** As a developer, I want a Money value object that supports arithmetic operations, so that I can perform calculations safely.

**Acceptance Criteria:**
- Amount and Currency properties
- Add/Subtract operations (same currency only)
- Multiply by scalar
- Currency mismatch throws exception

**Status:** New

##### Tasks:
| ID | Task | Status |
|----|------|--------|
| 2.4.1.1 | Create Money sealed class | New |
| 2.4.1.2 | Add Amount and Currency properties | New |
| 2.4.1.3 | Implement Add() method | New |
| 2.4.1.4 | Implement Subtract() method | New |
| 2.4.1.5 | Implement Multiply() method | New |
| 2.4.1.6 | Add currency mismatch validation | New |
| 2.4.1.7 | Create MoneyException | New |
| 2.4.1.8 | Write unit tests | New |

---

### Feature 2.5: Address Value Object

**Description:** Create an Address value object for physical addresses.

**Parent:** Epic 2

#### User Story 2.5.1: Create Address with Components

**Description:** As a developer, I want an Address value object with standard address components, so that address data is structured consistently.

**Acceptance Criteria:**
- Street, City, State/Province, PostalCode, Country
- Required field validation
- Formatted output

**Status:** New

##### Tasks:
| ID | Task | Status |
|----|------|--------|
| 2.5.1.1 | Create Address sealed class | New |
| 2.5.1.2 | Add address component properties | New |
| 2.5.1.3 | Implement Create() with validation | New |
| 2.5.1.4 | Add formatted ToString() | New |
| 2.5.1.5 | Create AddressException | New |
| 2.5.1.6 | Write unit tests | New |

---

## Epic 3: DevOps and CI/CD

**Description:** Set up continuous integration, automated testing, and package publishing.

**Acceptance Criteria:**
- Automated builds on commit
- Test execution with coverage reporting
- NuGet package publishing

---

### Feature 3.1: Azure DevOps Pipeline

**Description:** Create CI/CD pipeline for automated builds and tests.

**Parent:** Epic 3

#### User Story 3.1.1: Build Pipeline

**Description:** As a developer, I want automated builds on every commit, so that I catch build errors early.

**Acceptance Criteria:**
- Trigger on push to main/master
- Trigger on pull requests
- Build all projects in solution
- Fail on build errors

**Status:** New

##### Tasks:
| ID | Task | Status |
|----|------|--------|
| 3.1.1.1 | Create azure-pipelines.yml | New |
| 3.1.1.2 | Configure .NET 8.0 SDK | New |
| 3.1.1.3 | Add restore step | New |
| 3.1.1.4 | Add build step | New |
| 3.1.1.5 | Configure triggers | New |

#### User Story 3.1.2: Test Pipeline

**Description:** As a developer, I want automated test execution with coverage, so that I can track code quality.

**Acceptance Criteria:**
- Run all tests on build
- Generate coverage report
- Publish test results to Azure DevOps
- Fail pipeline on test failures

**Status:** New

##### Tasks:
| ID | Task | Status |
|----|------|--------|
| 3.1.2.1 | Add test step to pipeline | New |
| 3.1.2.2 | Configure code coverage (Coverlet) | New |
| 3.1.2.3 | Publish test results | New |
| 3.1.2.4 | Publish coverage results | New |

---

### Feature 3.2: NuGet Package

**Description:** Publish the library as a NuGet package.

**Parent:** Epic 3

#### User Story 3.2.1: Package Configuration

**Description:** As a developer, I want the library published as a NuGet package, so that others can easily consume it.

**Acceptance Criteria:**
- Proper package metadata
- XML documentation included
- Semantic versioning
- Published to NuGet.org or Azure Artifacts

**Status:** New

##### Tasks:
| ID | Task | Status |
|----|------|--------|
| 3.2.1.1 | Add package properties to .csproj | New |
| 3.2.1.2 | Enable XML documentation generation | New |
| 3.2.1.3 | Create package icon | New |
| 3.2.1.4 | Add LICENSE file | New |
| 3.2.1.5 | Configure pack in pipeline | New |
| 3.2.1.6 | Configure publish to feed | New |

---

## Epic 4: Documentation and Examples

**Description:** Create comprehensive documentation and example projects.

**Acceptance Criteria:**
- Clear README with quick start
- API documentation
- Example console application
- Wiki or docs site

---

### Feature 4.1: README Documentation

**Description:** Comprehensive README with installation, usage, and API reference.

**Parent:** Epic 4

**Status:** Done

---

### Feature 4.2: Console Demo Application

**Description:** Interactive demo showing value object usage patterns.

**Parent:** Epic 4

#### User Story 4.2.1: Update Demo Application

**Description:** As a developer, I want a demo application that showcases all value objects, so that I can see practical usage examples.

**Acceptance Criteria:**
- Demonstrate each value object
- Show validation in action
- Show equality comparison
- Show parsing and formatting

**Status:** Partially Done

##### Tasks:
| ID | Task | Status |
|----|------|--------|
| 4.2.1.1 | Create Currency demo section | Done |
| 4.2.1.2 | Create Price demo section | New |
| 4.2.1.3 | Create Email demo section | New |
| 4.2.1.4 | Create Money demo section | New |
| 4.2.1.5 | Add error handling examples | New |

---

### Feature 4.3: API Documentation

**Description:** XML documentation comments for all public members.

**Parent:** Epic 4

#### User Story 4.3.1: Add XML Documentation

**Description:** As a developer, I want XML comments on all public APIs, so that IntelliSense shows helpful information.

**Acceptance Criteria:**
- All public classes documented
- All public methods documented
- All public properties documented
- Examples where helpful

**Status:** New

##### Tasks:
| ID | Task | Status |
|----|------|--------|
| 4.3.1.1 | Document ValueObject base class | New |
| 4.3.1.2 | Document Currency class | New |
| 4.3.1.3 | Document CurrencyException class | New |
| 4.3.1.4 | Document new value objects as added | New |

---

## Summary

### Work Item Counts

| Type | Total | Done | In Progress | New |
|------|-------|------|-------------|-----|
| Epics | 4 | 0 | 1 | 3 |
| Features | 13 | 4 | 0 | 9 |
| User Stories | 15 | 5 | 0 | 10 |
| Tasks | 65 | 28 | 0 | 37 |

### Priority Order for New Work

1. **Feature 3.1** - Azure DevOps Pipeline (enables CI/CD)
2. **Feature 2.1** - Price Value Object (referenced in README)
3. **Feature 3.2** - NuGet Package
4. **Feature 2.4** - Money Value Object
5. **Feature 2.2** - Email Value Object
6. **Feature 2.3** - PhoneNumber Value Object
7. **Feature 2.5** - Address Value Object
8. **Feature 4.3** - API Documentation

---

## How to Create in Azure DevOps

1. Go to `https://dev.azure.com/tengtium-io/`
2. Create a new project (or use existing): **TestNest.ValueObjects**
3. Navigate to **Boards > Backlogs**
4. Create Epics first, then Features under each Epic
5. Create User Stories under Features
6. Create Tasks under User Stories
7. Use the status column to mark items as Done/New

### Recommended Board Columns

- New
- Active
- In Progress
- Code Review
- Done

### Area Paths (Optional)

- TestNest.ValueObjects\Core
- TestNest.ValueObjects\ValueObjects
- TestNest.ValueObjects\DevOps
- TestNest.ValueObjects\Documentation
