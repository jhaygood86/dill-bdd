Feature: Refund item

  Scenario: Jeff returns a faulty microwave
    Given Jeff has bought a microwave for $100
    And he has a receipt
    When he returns the microwave
    Then Jeff should be refunded $100
	And Jeff should not have a microwave

   Scenario: Jeff returns a faulty microwave without receipt
    Given Jeff has bought a microwave for $100
    And he forgot his receipt
    When he returns the microwave
    Then Jeff should be refunded $0
	And Jeff should have a microwave